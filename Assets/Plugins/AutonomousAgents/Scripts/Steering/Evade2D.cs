
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Evade a single, targeted, mobile entity.
    /// </summary>
    [DisallowMultipleComponent]
    public class Evade2D : SteeringBehavior2D
    {
        public EvadeSensor2D sensor;

        public override Vector2 SteerForce()
        {
            return SteerForce(Agent, sensor);
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, EvadeSensor2D sensor)
        {
            // Nothing setup to avoid.
            if (sensor.avoid == null) {
                return Vector2.zero;
            }

            Vector2 targetPosition = sensor.avoid.position;
            Vector2 targetVelocity = sensor.avoid.velocity;

            Vector2 toAvoider = targetPosition - (Vector2)agent.transform.position;
            float distanceToAvoid = toAvoider.magnitude;

            if (sensor.ShouldAvoid(distanceToAvoid)) {

                float lookAheadTime = distanceToAvoid / (agent.maxSpeed + targetVelocity.magnitude);
                Vector2 fleePosition = targetPosition + targetVelocity * lookAheadTime;

                return Flee2D.SteerForce(agent, fleePosition);
            }

            // Too far too evade
            else {
                return Vector2.zero;
            }
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(Agent, sensor);
        }

        public static void DrawGizmos(AutonomousAgent2D agent, EvadeSensor2D sensor)
        {
            if (sensor.avoid != null) {

                if (sensor.bUseRange) {
                    DrawUtil.DrawCircle(agent.transform.position, sensor.avoidRange, Color.red);
                }

                DrawUtil.DrawArrow(agent.transform.position, sensor.avoid.position, Color.green, 1f, true);
            }
        }
    }
}