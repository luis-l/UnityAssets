
using UnityEngine;

namespace AutonomousAgents
{
    /// <summary>
    /// Detects 2D colliders that are part of an autonmous agent object.
    /// </summary>
    public class AgentSensor2D : ObjectSensor2D<AutonomousAgent2D>
    {
        protected void Start()
        {
            filter = filterAgent;
        }

        /* 
         * NOTE:
         *  Assumes that the collider detected is in the same
         *  hierachy as the AutonomousAgent2D component.
         */
        private static AutonomousAgent2D filterAgent(Collider2D other)
        {
            return other.GetComponent<AutonomousAgent2D>();
        }
    }
}