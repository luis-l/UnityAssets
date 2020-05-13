
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Separates the agent from neighboring agents.
    /// </summary>
    [DisallowMultipleComponent]
    public class Separation2D : GroupSteering2D
    {
        public float detectionRadius = 1f;

        public override Vector2 SteerForce()
        {
            throw new System.NotImplementedException();
        }

        public static Vector2 SteerForce(AutonomousAgent2D agent, Collider2D[] colliders, int totalCount, float detectionRadius)
        {
            Vector2 steering = Vector2.zero;
            Vector2 agentPosition = agent.transform.position;
            Vector2 agentVelocity = agent.rigid.velocity;

            int count = 0;

            for(int i = 0; i < totalCount; ++i) {

                var other = colliders[i].GetComponent<AutonomousAgent2D>();

                if (other == agent) {
                    continue;
                }

                Vector2 toAgent = agentPosition - (Vector2)other.transform.position;
                float distance = toAgent.magnitude;

                if (distance < detectionRadius) {

                    count += 1;

                    // Force is inversely proportional to the distance to the other agent.
                    steering += toAgent.normalized / distance;
                }
            }

            if (count > 0) {
                return (steering.normalized * agent.maxSpeed) - agentVelocity;
            }

            return Vector2.zero;
        }
    }
}