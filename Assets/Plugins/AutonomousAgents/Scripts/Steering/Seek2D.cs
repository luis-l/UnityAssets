
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Move towards a position.
    /// </summary>
    [DisallowMultipleComponent]
    public class Seek2D : SteeringBehavior2D
    {
        [Tooltip("The target location to seek.")]
        public Vector2 target;

        public override Vector2 SteerForce()
        {
            return SteerForce(target, Agent);
        }

        public static Vector2 SteerForce(Vector2 target, AutonomousAgent2D agent)
        {
            Vector2 agentPosition = agent.transform.position;
            Vector2 desired = (target - agentPosition).normalized * agent.maxSpeed;

            return desired - agent.rigid.velocity;
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(target, Agent);
        }

        public static void DrawGizmos(Vector2 target, AutonomousAgent2D agent)
        {
            // Draw the arrow pointing to the target position.
            DrawUtil.DrawArrow(agent.transform.position, target, Color.green, 0.8f);

            // Emphasize the target position.
            DrawUtil.DrawCircle(target, 0.15f, Color.green);
        }
    }
}