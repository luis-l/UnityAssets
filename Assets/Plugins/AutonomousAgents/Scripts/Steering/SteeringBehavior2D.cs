
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Base class for all steering behaviors.
    /// </summary>
    [RequireComponent(typeof(AutonomousAgent2D))]
    public abstract class SteeringBehavior2D : MonoBehaviour
    {
        public bool active = true;
        public float steerScaling = 1f;

        private AutonomousAgent2D _agent;
        protected AutonomousAgent2D Agent
        {
            get
            {
                cacheAgent();
                return _agent;
            }
        }

        public enum Priority { DEFAULT, HIGH, VERY_HIGH };
        protected Priority _priority = Priority.DEFAULT;

        public Priority SteerPriority
        {
            get { return _priority; }
        }

        /// <summary>
        /// Prefix to help identify which game objects are sensors.
        /// </summary>
        public const string kSensorPrefix = "AAS:";

        public abstract Vector2 SteerForce();

        private void cacheAgent()
        {
            if (_agent == null) {

                _agent = GetComponent<AutonomousAgent2D>();

                if (_agent == null) {
                    throw new UnityException("Autonamous Agent not found!");
                }
            }
        }

        /// <summary>
        /// Initializes and sets up a sensor child game object.
        /// </summary>
        /// <typeparam name="SType">Sensor type.</typeparam>
        /// <typeparam name="DType"> Type of component detected by the sensor.</typeparam>
        /// <typeparam name="CType">Collider Type</typeparam>
        /// <param name="sensor"></param>
        /// <param name="sensorCollider">The collider that will be used to trigger the sensor.</param>
        protected void initSensor<SType, DType, CType>(out SType sensor, out CType sensorCollider)

            where DType : Component
            where SType : ObjectSensor2D<DType>
            where CType : Collider2D
        {

            string detectTypeName = typeof(DType).Name;
            string sensorName = kSensorPrefix + detectTypeName + GetInstanceID().ToString();

            var root = Agent.transform;
            var sensorChild = root.FindChild(sensorName);

            // Sensor not yet added, create a child with sensor component.
            if (sensorChild == null) {

                GameObject sensorGameObject = new GameObject(sensorName);
                sensorGameObject.transform.parent = root;
                sensorGameObject.transform.localPosition = Vector3.zero;

                sensor = sensorGameObject.AddComponent<SType>();

                // The collider for the sensor.
                sensorCollider = sensorGameObject.AddComponent<CType>();
                sensorCollider.isTrigger = true;

                // Sensor ignores colliders associated with the agent,
                // since it is only meant to detect other objects.
                var agentColliders = Agent.GetComponentsInChildren<Collider2D>();
                foreach (var otherCollider in agentColliders) {

                    if (sensorCollider != otherCollider) {
                        Physics2D.IgnoreCollision(sensorCollider, otherCollider);
                    }
                }
            }

            // Set the references if the object already exists.
            else {
                sensor = sensorChild.gameObject.GetComponent<SType>();
                sensorCollider = sensorChild.gameObject.GetComponent<CType>();
            }
        }
    }
}