
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steers away from a position.
    /// </summary>
    [DisallowMultipleComponent]
    public class Flee2D : SteeringBehavior2D
    {
        /// <summary>
        /// The target to flee from.
        /// </summary>
        public Vector2 target;

        public override Vector2 SteerForce()
        {
            return SteerForce(Agent, target);
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, Vector2 target)
        {
            Vector2 desired = ((Vector2)agent.transform.position - target).normalized * agent.maxSpeed;
            return desired - agent.rigid.velocity;
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(Agent, target);
        }

        public static void DrawGizmos(AutonomousAgent2D agent, Vector2 target)
        {
            // Draw the arrow towards the target.
            DrawUtil.DrawArrow(agent.transform.position, target, Color.green, 0.8f, true);

            // Emphasize the fleeing position.
            DrawUtil.DrawCircle(target, 0.15f, Color.green);
        }
    }
}