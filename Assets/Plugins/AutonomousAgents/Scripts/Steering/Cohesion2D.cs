
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steers the agent toward the center "mass" of its neighbors.
    /// In other words, towards the average position of the neighbors.
    /// </summary>
    [DisallowMultipleComponent]
    public class Cohesion2D : GroupSteering2D
    {
        public override Vector2 SteerForce()
        {
            throw new System.NotImplementedException();
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, Collider2D[] colliders, int totalCount, float detectionRadius)
        {
            Vector2 positionsSum = Vector2.zero;
            Vector2 steering = Vector2.zero;
            Vector2 agentPosition = agent.transform.position;

            int count = 0;
            float radiusSq = detectionRadius * detectionRadius;

            for (int i = 0; i < totalCount; ++i) {

                var other = colliders[i].GetComponent<AutonomousAgent2D>();
                
                if (other == agent) {
                    continue;
                }

                if (agentPosition.WithinDistanceSq(other.transform.position, radiusSq)) {
                    count += 1;
                    positionsSum += (Vector2)other.transform.position;
                }
            }

            if (count > 0) {

                // The center "mass".
                Vector2 averagePosition = positionsSum / count;
                return Seek2D.SteerForce(averagePosition, agent);
            }

            return Vector2.zero;
        }
    }
}