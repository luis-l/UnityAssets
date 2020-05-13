
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steer away from elongated obstacles to avoid collision.
    /// </summary>
    [DisallowMultipleComponent]
    public class WallAvoidance2D : SteeringBehavior2D
    {

        public float mainWhiskerLength = 1f;
        public float sideWhiskerLength = 1f;
        public float sideWhiskerAngle = 45f;

        private RaycastHit2D[] _hits = new RaycastHit2D[3];

        private RaycastHit2D _closestHit;
        private float _penetrationDepth;

        void Start()
        {
            _priority = Priority.VERY_HIGH;
        }

        public override Vector2 SteerForce()
        {
            _closestHit = getNearestHit();

            if (_closestHit.collider == null) {
                return Vector2.zero;
            }

            return _closestHit.normal * _penetrationDepth;
        }

        private RaycastHit2D getNearestHit()
        {
            RaycastHit2D nearestHit = default(RaycastHit2D);

            Vector2 agentPosition = Agent.transform.position;

            _hits[0] = Physics2D.Raycast(agentPosition, mainWhiskerDirection, mainWhiskerLength);
            _hits[1] = Physics2D.Raycast(agentPosition, rightWhiskerDirection, sideWhiskerLength);
            _hits[2] = Physics2D.Raycast(agentPosition, leftWhiskerDirection, sideWhiskerLength);

            float closestDistance = float.MaxValue;

            for (int i = 0; i < _hits.Length; ++i) {

                RaycastHit2D hit = _hits[i];

                // Hit detected nothing, check the other ray hits.
                if (hit.collider == null) {
                    continue;
                }

                float distanceToHit = Vector2.Distance(agentPosition, hit.point);

                // New closest found.
                if (distanceToHit < closestDistance) {

                    closestDistance = distanceToHit;
                    nearestHit = hit;

                    // If i == 0, then use the main whisker length else use the sidw whisker length.
                    float rayLength = i == 0 ? mainWhiskerLength : sideWhiskerLength;

                    // Store the penetration depth to later help calculate the steering force.
                    _penetrationDepth = rayLength - hit.distance;
                }
            }

            return nearestHit;
        }

        private Vector2 mainWhiskerDirection
        {
            get { return Agent.ForwardDirection; }
        }

        private Vector2 rightWhiskerDirection
        {
            get { return Agent.ForwardDirection.Rotate(-sideWhiskerAngle); }
        }

        private Vector2 leftWhiskerDirection
        {
            get { return Agent.ForwardDirection.Rotate(sideWhiskerAngle); }
        }

        void OnDrawGizmosSelected()
        {
            Vector2 agentPosition = Agent.transform.position;

            // Draw the whiskers
            DrawUtil.DrawRay(agentPosition, mainWhiskerDirection, mainWhiskerLength, Color.red);
            DrawUtil.DrawRay(agentPosition, rightWhiskerDirection, sideWhiskerLength, Color.red);
            DrawUtil.DrawRay(agentPosition, leftWhiskerDirection, sideWhiskerLength, Color.red);

            // Draw the hit that the agent is trying to avoid.
            if (_closestHit.collider != null) {
                DrawUtil.DrawLine(agentPosition, _closestHit.point, Color.red);
                DrawUtil.DrawCrosshair(_closestHit.point, 0.3f, Color.red);

                DrawUtil.DrawRay(_closestHit.point, _closestHit.normal, 2f, Color.cyan);
            }
        }

    }
}