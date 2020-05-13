
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Smoothly steer about in a random manner.
    /// </summary>
    [DisallowMultipleComponent]
    public class Wander2D : SteeringBehavior2D
    {

        /// <summary>
        /// The distance between the agent and the wander radius.
        /// </summary>
        public float wanderDistance = 1f;

        /// <summary>
        /// The radius of the wander circle.
        /// </summary>
        public float wanderRadius = 1f;

        private Vector2 _wanderTarget;
        private Vector2 _wanderCenter;
        private Vector2 _wanderDirection = Vector2.right;

        public float wanderChangeRate = 1f;
        private float _wanderChangeTimer = 0f;

        protected void Start()
        {
            changeWanderDirection();
        }

        protected void Update()
        {
            updateWander();
        }

        public override Vector2 SteerForce()
        {
            return Seek2D.SteerForce(_wanderTarget, Agent);
        }

        private void updateWander()
        {
            _wanderChangeTimer += Time.deltaTime;

            _wanderCenter = (Vector2)Agent.transform.position + Agent.ForwardDirection * wanderDistance;
            _wanderTarget = _wanderCenter + _wanderDirection * wanderRadius;

            // Change the wonder target.
            if (_wanderChangeTimer >= wanderChangeRate) {

                // Reset timer for next update.
                _wanderChangeTimer = 0f;

                // Change the wander direction which in turn changes the wander target.
                changeWanderDirection();
            }
        }

        private void changeWanderDirection()
        {
            _wanderDirection = Random.insideUnitCircle.normalized;
        }

        void OnValidate()
        {
            updateWander();
        }

        void OnDrawGizmosSelected()
        {
            if (!active) {
                return;
            }

            if (transform.hasChanged) {
                updateWander();
            }

            // Draw connection between wander circle and the agent.
            DrawUtil.DrawLine(Agent.transform.position, _wanderCenter, Color.green);

            // Draw the wander properies, such as the target, radius, and wander center.
            DrawUtil.DrawLine(_wanderCenter, _wanderTarget, Color.green);
            DrawUtil.DrawCrosshair(_wanderTarget, 1f, Color.yellow);
            DrawUtil.DrawCircle(_wanderCenter, wanderRadius, Color.green);
        }
    }
}