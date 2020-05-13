
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Stores sensor data used by the arrive steering behavior.
    /// </summary>
    [System.Serializable]
    public class ArriveSensor2D
    {

        /// <summary>
        /// The target position to travel to.
        /// </summary>
        public Vector2 target;

        /// <summary>
        /// How fast to slow down when approaching the target.
        /// </summary>
        public float decelerationFactor = 0.1f;

        /// <summary>
        /// The distance from the target position to start slowing down.
        /// </summary>
        public float slowRadius = 2f;

        /// <summary>
        /// The distance threshold to stop steering forces.
        /// </summary>
        public float stopRadius = 0.05f;

        /// <summary>
        /// The velocity threshold to come to a complete stop.
        /// </summary>
        public float stopSpeed = 0.4f;
    }
}