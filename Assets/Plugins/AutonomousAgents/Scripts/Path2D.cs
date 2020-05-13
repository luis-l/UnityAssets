
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A path composed of linear segments.
    /// </summary>
    /// 
    [System.Serializable]
    public class Path2D
    {
        public bool bLoop = false;

        [Tooltip("The distance to the waypoint that counts as reached.")]
        public float nearWaypointThresh = 1f;

        public Vector2[] waypoints;

        private int _currentWaypointIndex = 0;

        public Path2D()
        {

        }

        public Path2D(Vector2[] waypoints)
        {
            this.waypoints = waypoints;
        }

        public void SetNextWaypoint()
        {
            // If we are not done yet, then go to the next paypoint.
            if (!Finished()) {
                _currentWaypointIndex += 1;
            }

            // Loop back to the beginning.
            if (bLoop && _currentWaypointIndex == Size) {
                _currentWaypointIndex = 0;
            }

            // If we are not looping and the current waypoint index is
            // out of bounds then reset back to 0.
            if (!bLoop && _currentWaypointIndex >= Size) {
                _currentWaypointIndex = 0;
            }
        }

        /// <summary>
        /// Resets the current waypoint to the first waypoint.
        /// </summary>
        public void Reset()
        {
            _currentWaypointIndex = 0;
        }

        public Vector2 CurrentWaypoint()
        {
            return waypoints[_currentWaypointIndex];
        }

        public bool Finished()
        {
            // We are never done if we are looping.
            if (bLoop) {
                return false;
            }

            else {

                // The last waypoint was reached.
                return _currentWaypointIndex == Size - 1;
            }
        }

        /// <summary>
        /// Checks if the position and current waypoint are near.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool NearCurrentWaypoint(Vector2 position)
        {
            float thresholdSq = nearWaypointThresh * nearWaypointThresh;
            return CurrentWaypoint().WithinDistanceSq(position, thresholdSq);
        }

        /// <summary>
        /// The number of waypoints the path contains.
        /// </summary>
        public int Size
        {
            get { return waypoints.Length; }
        }

        public Vector2 LastWaypoint
        {
            get { return waypoints[Size - 1]; }
        }

        public Vector2 FirstWaypoint
        {
            get { return waypoints[0]; }
        }

        public void DrawGizmos()
        {
            for (int i = 0; i < Size - 1; ++i) {

                Vector2 start = waypoints[i];
                Vector2 end = waypoints[i + 1];

                // Draw a line segment between the waypoints.
                DrawUtil.DrawArrow(start, end, Color.cyan, 0.35f);

                // Draw the near threshold.
                DrawUtil.DrawCircle(start, nearWaypointThresh, Color.blue);
            }

            // Draw the near threshold for last waypoint since the loop only goes to Length-1.
            DrawUtil.DrawCircle(LastWaypoint, nearWaypointThresh, Color.blue);

            // Draw a line segment between the end and start waypoints.
            if (bLoop) {
                DrawUtil.DrawArrow(LastWaypoint, FirstWaypoint, Color.cyan, 0.35f);
            }
        }

        public static Vector2[] BoxWaypoints(Vector2 center, float width, float height)
        {
            float wHalf = width / 2;
            float hHalf = height / 2;

            Vector2 topLeft = center + new Vector2(-wHalf, hHalf);
            Vector2 topRight = center + new Vector2(wHalf, hHalf);

            Vector2 bottomRight = center + new Vector2(wHalf, -hHalf);
            Vector2 bottomLeft = center - new Vector2(wHalf, hHalf);

            Vector2[] waypoints = new Vector2[4];

            waypoints[0] = topLeft;
            waypoints[1] = topRight;
            waypoints[2] = bottomRight;
            waypoints[3] = bottomLeft;

            return waypoints;
        }
    }
}
