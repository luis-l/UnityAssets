
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Pursue a mobile target by intercepting its predicted future position.
    /// </summary>
    [DisallowMultipleComponent]
    public class Pursuit2D : SteeringBehavior2D
    {

        /// <summary>
        /// The entity to pursue.
        /// </summary>
        public Rigidbody2D targetRigid;

        private Vector2 _futureSeekCache;

        public override Vector2 SteerForce()
        {
            Vector2 toTarget = targetRigid.position - (Vector2)Agent.transform.position;
            Vector2 targetVelocityDir = targetRigid.velocity.normalized;

            float relativeForward = Vector2.Dot(Agent.ForwardDirection, targetVelocityDir);

            // If the target is head on, then just seek towards it.
            if (Vector2.Dot(toTarget, Agent.ForwardDirection) > 0
                && relativeForward < -0.95f) {

                _futureSeekCache = targetRigid.position;

                return Seek2D.SteerForce(targetRigid.position, Agent);
            }

            float lookAheadTime = toTarget.magnitude / (Agent.maxSpeed + targetRigid.velocity.magnitude);

            Vector2 future = targetRigid.position + targetRigid.velocity * lookAheadTime;
            _futureSeekCache = future;

            return Seek2D.SteerForce(future, Agent);
        }

        // This is to help objects that need to turn in order to start moving in a certain position.
        // Takes in the dot product between the forward direction of the agent and the target position.
        /*private float turnTime(float forwardTargetDot)
        {
         * // The higher the turn rate, the higher the coefficient should be.
            float coefficient = 0.5;
         * 
            return (forwardTargetDot - 1f) * -coefficient;
        }*/

        void OnValidate()
        {
            if (targetRigid) {
                _futureSeekCache = targetRigid.position;
            }
        }
        
        void OnDrawGizmosSelected()
        {
            if (targetRigid) {
                Seek2D.DrawGizmos(_futureSeekCache, Agent);
                DrawUtil.DrawLine(_futureSeekCache, targetRigid.position, Color.green);
            }
        }
    }
}