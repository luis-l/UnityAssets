
using System.Collections.Generic;
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// An agent automatically controlled by steering behaviors.
    /// </summary>
    public class AutonomousAgent2D : Agent2D
    {

        private List<SteeringBehavior2D> _steeringBehaviors = new List<SteeringBehavior2D>();

        private Vector2 _totalSteering = Vector2.zero;

        void Start()
        {
            cacheSteeringComponents();
        }

        protected override void Update()
        {
            base.Update();
            _totalSteering = getTotalSteering();
        }

        protected void FixedUpdate()
        {
            applySteering(_totalSteering);
        }

        public T AddSteering<T>() where T : SteeringBehavior2D
        {
            var sb = gameObject.AddComponent<T>();
            _steeringBehaviors.Add(sb);

            return sb;
        }

        private void cacheSteeringComponents()
        {
            foreach (SteeringBehavior2D sb in GetComponents<SteeringBehavior2D>()) {
                _steeringBehaviors.Add(sb);
            }
        }

        /// <summary>
        /// Combine all the steer forces and apply them to the rigidbody.
        /// </summary>
        private Vector2 getTotalSteering()
        {
            Vector2 steerForce = Vector2.zero;

            for (int i = 0; i < _steeringBehaviors.Count; ++i) {

                SteeringBehavior2D sb = _steeringBehaviors[i];

                // Ignore inactive steering behaviors.
                if (!sb.active) {
                    continue;
                }

                Vector2 force = Vector2.ClampMagnitude(sb.SteerForce() * sb.steerScaling, maxForce);

                if (sb.SteerPriority == SteeringBehavior2D.Priority.VERY_HIGH && force.magnitude >= 0.005f) {
                    steerForce = force;
                    break;
                }

                else if (sb.SteerPriority == SteeringBehavior2D.Priority.HIGH && force.magnitude >= 0.005f) {
                    steerForce = force;
                    break;
                }
                else {
                    steerForce += Vector2.ClampMagnitude(force, maxForce);
                }
            }

            return steerForce;
        }

        void applySteering(Vector2 steerForce)
        {
            if (steerForce != Vector2.zero) {
                rigid.AddForce(steerForce);
            }

            capVelocity();
        }
    }
}