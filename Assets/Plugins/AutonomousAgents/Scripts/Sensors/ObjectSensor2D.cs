
using System.Collections.Generic;
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A sensor that can detect multiple objects of some type.
    /// </summary>
    public abstract class ObjectSensor2D<T> : MonoBehaviour where T : Component
    {

        private HashSet<T> _detected = new HashSet<T>();
        public HashSet<T> Detected { get { return _detected; } }

        public bool bDetectTriggers = false;

        private System.Action<Collider2D> triggerEnter;
        private System.Action<Collider2D> triggerExit;

        /// <summary>
        /// Filters colliders so only those with a T type associated pass.
        /// </summary>
        public System.Func<Collider2D, T> filter;

        protected void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Sensor");

            // If T is a collider type, then just simply add/remove to the detected.
            // There is no need to find it since the trigger functions already provide
            // a reference to the colliders.
            if (typeof(T) == typeof(Collider2D)) {

                triggerEnter = (other) =>
                {
                    _detected.Add(other as T);
                };

                triggerExit = (other) =>
                {
                    if (!_detected.Remove(other as T)) {
                        Debug.LogError(gameObject.name + " failed to remove: " + other.name);
                    }
                };
            }

            // For this case, we must find the component of type T in order to
            // filter it into the detected set.
            else {

                triggerEnter = (other) =>
                {
                    var c = filter(other);
                    if (c) _detected.Add(c);
                };

                triggerExit = (other) =>
                {
                    var c = filter(other);

                    if (c && !_detected.Remove(c)) {
                        Debug.LogError(gameObject.name + " failed to remove: " + other.name);
                    }
                };
            }
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            // If detect triggers is ON, then it detects any collider.
            // If detect triggers is OFF, then it detect non-triggers.
            if (bDetectTriggers || !other.isTrigger) {

                triggerEnter(other);
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (bDetectTriggers || !other.isTrigger) {
                triggerExit(other);
            }
        }

        /// <summary>
        /// Transform the point from world space to the sensor's local space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 World2Local(Vector2 point)
        {
            return transform.InverseTransformPoint(point);
        }

        /// <summary>
        /// Transform the point from the sensor's local space to world space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 Local2World(Vector2 point)
        {
            return transform.TransformPoint(point);
        }

        /// <summary>
        /// Transform the point from the sensor's local space to world space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 Local2World(float x, float y)
        {
            return Local2World(new Vector2(x, y));
        }

        public Vector2 Local2WorldDirection(Vector2 v)
        {
            return transform.TransformDirection(v);
        }
    }
}