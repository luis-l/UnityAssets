
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Follow a target.
    /// </summary>
    [DisallowMultipleComponent]
    public class Homing2D : SteeringBehavior2D
    {

        public Transform target;

        public override Vector2 SteerForce()
        {
            if (target != null) {
                return Seek2D.SteerForce(target.position, Agent);
            }

            return Vector2.zero;
        }

        void OnDrawGizmosSelected()
        {
            if (target != null) {
                Seek2D.DrawGizmos(target.position, Agent);
            }
        }
    }
}
