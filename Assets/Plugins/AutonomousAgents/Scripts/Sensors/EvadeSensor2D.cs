
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A single targeting sensor to avoid a mobile object.
    /// </summary>
    [System.Serializable]
    public class EvadeSensor2D
    {
        public Rigidbody2D avoid;

        /// <summary>
        /// The distance to the target to start fleeing.
        /// </summary>
        public float avoidRange = 3f;

        public bool bUseRange = true;

        public bool ShouldAvoid(float distanceToAvoid)
        {
            if (bUseRange) {
                return distanceToAvoid < avoidRange;
            }

            // Always evade if range is disabled.
            else {
                return true;
            }
        }

        /// <summary>
        /// If the agent should evade the target rigid.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public bool ShouldAvoid(AutonomousAgent2D agent)
        {
            if (avoid != null) {
                return ShouldAvoid(Vector2.Distance(agent.transform.position, avoid.position));
            }

            // If there is nothing to avoid, then no need to avoid.
            return false;
        }
    }
}