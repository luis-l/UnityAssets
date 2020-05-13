
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steer towards the mouse position.
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowMouse2D : SteeringBehavior2D
    {

        public float randomDisplacement = 2f;

        public override Vector2 SteerForce()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition += Random.insideUnitCircle.normalized * randomDisplacement;

            return Seek2D.SteerForce(mousePosition, Agent);
        }
    }
}