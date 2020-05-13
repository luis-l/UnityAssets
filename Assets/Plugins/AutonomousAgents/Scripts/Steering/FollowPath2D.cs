
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Follow a sequence of waypoints.
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowPath2D : SteeringBehavior2D
    {
        public Path2D path;
        public ArriveSensor2D arriveSensor;

        private bool _bUsingArrive = true;

        protected void Awake()
        {
            if (path == null) {
                path = new Path2D();
            }

            if (arriveSensor == null) {
                arriveSensor = new ArriveSensor2D();
            }
        }

        protected void Update()
        {
            // This is here to be independent of steering.
            // The next waypoint should be regardless if a higher priority
            // steering (like avoidance) causes path following to be skipped.
            if (path.NearCurrentWaypoint(Agent.transform.position)) {
                path.SetNextWaypoint();
            }
        }

        public override Vector2 SteerForce()
        {
            if (path.Size == 0) {
                _bUsingArrive = false;
                return Vector2.zero;
            }

            else if (path.Size == 1) {
                _bUsingArrive = true;
                arriveSensor.target = path.FirstWaypoint;
                Arrive2D.SteerForce(Agent, arriveSensor);
            }

            if (!path.Finished()) {
                _bUsingArrive = false;
                return Seek2D.SteerForce(path.CurrentWaypoint(), Agent);
            }

            else {
                _bUsingArrive = true;
                arriveSensor.target = path.CurrentWaypoint();
                return Arrive2D.SteerForce(Agent, arriveSensor);
            }
        }

        public void OnValidate()
        {
            validatePathGizmos();
        }

        private void validatePathGizmos()
        {
            // Here we set the target to the first waypoint in order
            // to show the use the beginning of the path.
            if (path.waypoints != null && path.Size > 0) {
                arriveSensor.target = path.FirstWaypoint;
            }

            // If no path exists then just target itself.
            else {
                arriveSensor.target = Agent.transform.position;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (path.waypoints != null && path.Size != 0) {

                path.DrawGizmos();

                // Draw the arrival towards the last end point (if no looping)
                if (_bUsingArrive) {
                    Arrive2D.DrawGizmos(Agent, arriveSensor);
                }

                // Show which current waypoint we are traveling to.
                else {
                    Seek2D.DrawGizmos(path.CurrentWaypoint(), Agent);
                }
            }
        }
    }
}