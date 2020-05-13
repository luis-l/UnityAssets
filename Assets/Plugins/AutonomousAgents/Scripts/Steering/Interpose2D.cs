
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steers towards the midpoint of two mobile targets.
    /// </summary>
    [DisallowMultipleComponent]
    public class Interpose2D : SteeringBehavior2D
    {

        // The targets to interpose.
        public Rigidbody2D targetA;
        public Rigidbody2D targetB;

        public ArriveSensor2D sensor;

        public override Vector2 SteerForce()
        {
            // Invalid targets.
            if (!targetsAreValid()) {
                return Vector2.zero;
            }

            Vector2 midpoint = (targetA.position + targetB.position) / 2f;

            float timeToReachMidpoint = Vector2.Distance(Agent.transform.position, midpoint) / Agent.maxSpeed;

            // Assume the targets will continue moving linearly in order to predict their future positions.
            Vector2 futurePositionA = targetA.position + targetA.velocity * timeToReachMidpoint;
            Vector2 futurePositionB = targetB.position + targetB.velocity * timeToReachMidpoint;

            Vector2 futureMidpoint = (futurePositionA + futurePositionB) / 2f;

            sensor.target = futureMidpoint;

            return Arrive2D.SteerForce(Agent, sensor);
        }

        void OnDrawGizmosSelected()
        {
            if (targetsAreValid()) {

                Arrive2D.DrawGizmos(Agent, sensor);

                Vector2 agentPosition = Agent.transform.position;

                // Draw the association of the targets to interpose.
                DrawUtil.DrawLine(agentPosition, targetA.position, Color.green);
                DrawUtil.DrawLine(agentPosition, targetB.position, Color.green);
            }
        }

        void OnValidate()
        {
            sensor.target = Agent.transform.position;
        }

        private bool targetsAreValid()
        {
            return targetA != null && targetB != null;
        }
    }
}