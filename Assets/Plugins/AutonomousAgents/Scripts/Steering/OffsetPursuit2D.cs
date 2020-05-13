
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Follow an other agent at a distance offset.
    /// </summary>
    [DisallowMultipleComponent]
    public class OffsetPursuit2D : SteeringBehavior2D
    {

        public Agent2D leader;

        [Tooltip("The offset position relative to the leader.")]
        public Vector2 localOffset;

        public ArriveSensor2D arriveSensor;

        public override Vector2 SteerForce()
        {
            if (leader == null) {
                return Vector2.zero;
            }

            // Convert the offset position into world coordinates.
            Vector2 worldOffset = leader.graphicTransform.TransformPoint(localOffset);
            Vector2 toOffset = worldOffset - (Vector2)Agent.transform.position;

            Vector2 leaderVelocity = leader.rigid.velocity;

            float lookAheadTime = toOffset.magnitude / (Agent.maxSpeed + leaderVelocity.magnitude);

            Vector2 futurePosition = worldOffset + leaderVelocity * lookAheadTime;
            arriveSensor.target = futurePosition;

            return Arrive2D.SteerForce(Agent, arriveSensor);
        }

        void OnValidate()
        {
            if (leader != null) {
                arriveSensor.target = leader.transform.position;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (leader != null) {

                Vector2 worldOffset = leader.graphicTransform.TransformPoint(localOffset);
                DrawUtil.DrawCrosshair(worldOffset, 1f, Color.white);

                Arrive2D.DrawGizmos(Agent, arriveSensor);
            }
        }
    }
}