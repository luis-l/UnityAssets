
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Aligns the agent's forward direction along with its neighbors.
    /// </summary>
    [DisallowMultipleComponent]
    public class Alignment2D : GroupSteering2D
    {
        public override Vector2 SteerForce()
        {
            throw new System.NotImplementedException();
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, Collider2D[] colliders, int totalCount, float detectionRadius)
        {
            Vector2 forwardsSum = Vector2.zero;
            Vector2 agentPosition = agent.transform.position;
            Vector2 agentForward = agent.ForwardDirection;

            int count = 0;
            float radiusSq = detectionRadius * detectionRadius;

            for (int i = 0; i < totalCount; ++i) {

                var other = colliders[i].GetComponent<AutonomousAgent2D>();

                if (other == agent) {
                    continue;
                }

                if (agentPosition.WithinDistanceSq(other.transform.position, radiusSq)) {
                    count += 1;
                    forwardsSum += other.ForwardDirection;
                }
            }

            if (count > 0) {

                Vector2 averageForward = (forwardsSum / count) - agentForward;
                return averageForward;
            }

            return Vector2.zero;
        }
    }
}