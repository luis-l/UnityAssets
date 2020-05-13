using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Gently arrive towards the target.
    /// </summary>
    [DisallowMultipleComponent]
    public class Arrive2D : SteeringBehavior2D
    {
        [Tooltip("Sensor data used to arrive at the target position.")]
        public ArriveSensor2D sensor;

        public override Vector2 SteerForce()
        {
            return SteerForce(Agent, sensor);
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, ArriveSensor2D sensor)
        {
            Vector2 desired = sensor.target - (Vector2)agent.transform.position;
            float distanceToTarget = desired.magnitude;

            desired.Normalize();

            // Stop completely.
            if (shouldStopSteering(distanceToTarget, sensor)) {
                if (shouldStopCompletely(agent, sensor)) {
                    agent.rigid.velocity = Vector2.zero;
                }
                return Vector2.zero;
            }

            // Slow down.
            else if (shouldSlowDown(distanceToTarget, sensor)) {

                float speed = agent.maxSpeed * (distanceToTarget / sensor.slowRadius);
                desired *= speed;
            }

            // Full speed.
            else {
                desired *= agent.maxSpeed;
            }

            return (desired - agent.rigid.velocity) * (1f / sensor.decelerationFactor);
        }

        private static bool shouldSlowDown(float distanceToTarget, ArriveSensor2D sensor)
        {
            return distanceToTarget <= sensor.slowRadius;
        }

        private static bool shouldStopSteering(float distanceToTarget, ArriveSensor2D sensor)
        {
            return distanceToTarget <= sensor.stopRadius;
        }

        private static bool shouldStopCompletely(AutonomousAgent2D agent, ArriveSensor2D sensor)
        {
            var vel = agent.rigid.velocity;
            var sqStop = sensor.stopSpeed * sensor.stopSpeed;
            return !vel.IsZero() && vel.sqrMagnitude < sqStop;
        }

        public static void DrawGizmos(AutonomousAgent2D agent, ArriveSensor2D sensor)
        {
            // Draw the slow radius
            DrawUtil.DrawCircle(agent.transform.position, sensor.slowRadius, Color.green);

            // Draw the stop radius
            DrawUtil.DrawCircle(agent.transform.position, sensor.stopRadius, Color.green);

            // Draw a line between the agent and its target.
            DrawUtil.DrawArrow(agent.transform.position, sensor.target, Color.green, 1f);

            // Draw the target position
            DrawUtil.DrawCircle(sensor.target, 0.15f, Color.green);
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(Agent, sensor);
        }
    }
}