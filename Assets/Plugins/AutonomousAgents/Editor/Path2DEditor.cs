
using UnityEngine;
using UnityEditor;

namespace AutonomousAgents
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(FollowPath2D))]
    public class Path2DEditor : Editor
    {
        FollowPath2D _pathFollow;
        Vector3 _waypointHandleSize;

        void OnEnable()
        {
            _pathFollow = target as FollowPath2D;
            _waypointHandleSize = Vector3.one;
        }

        void OnSceneGUI()
        {
            waypointHandles();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            handleWaypointOperations();
        }

        private void handleWaypointOperations()
        {
            
            var waypoints = _pathFollow.path.waypoints;
            Vector2 position = _pathFollow.transform.position;

            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Reset waypoints local position.") && validPath()) {

                for (int i = 0; i < waypoints.Length; ++i) {
                    waypoints[i] = position;
                }
            }

            else if (GUILayout.Button("Zero waypoints.") && validPath()) {

                for (int i = 0; i < waypoints.Length; ++i) {
                    waypoints[i] = Vector2.zero;
                }
            }

            else if (GUILayout.Button("Box Path")) {
                _pathFollow.path.waypoints = Path2D.BoxWaypoints(position, 8f, 4f);
            }

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(_pathFollow);
            }
        }

        private void waypointHandles()
        {
            if (!validPath()) {
                return;
            }

            EditorGUI.BeginChangeCheck();

            Handles.color = Color.cyan;
            var waypoints = _pathFollow.path.waypoints;
            int waypointCount = waypoints.Length;

            for (int i = 0; i < waypointCount; ++i) {

                Vector2 waypoint = waypoints[i];
                waypoints[i] = Handles.FreeMoveHandle(waypoint, Quaternion.identity, 0.3f, _waypointHandleSize, Handles.CircleHandleCap);
            }

            if (EditorGUI.EndChangeCheck()) {

                _pathFollow.OnValidate();
                EditorUtility.SetDirty(_pathFollow);
            }
        }

        private bool validPath()
        {
            return _pathFollow.path != null && _pathFollow.path.waypoints != null;
        }
    }
}
