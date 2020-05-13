
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A helper drawing utility.
    /// </summary>
    public static class DrawUtil
    {

        public static void DrawCircle(Vector2 center, float radius, Color color)
        {
            var originColor = Gizmos.color;

            Gizmos.color = color;

            Vector2 start = center + Vector2.right * radius;

            float segmentsRespectToCameraSize = 200f / Camera.main.orthographicSize;

            int segments = (int)Mathf.Clamp(segmentsRespectToCameraSize, 10f, 50f);

            float angleDelta = 2f * Mathf.PI / segments;

            for (int i = 0; i <= segments; ++i) {

                float x = Mathf.Cos(angleDelta * i) * radius;
                float y = Mathf.Sin(angleDelta * i) * radius;

                Vector2 end = center + new Vector2(x, y);

                Gizmos.DrawLine(start, end);


                start = end;
            }

            Gizmos.color = originColor;
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            var originColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(start, end);

            Gizmos.color = originColor;
        }

        public static void DrawCrosshair(Vector2 center, float length, Color color, float zRotation = 0f)
        {
            float halfLen = length / 2f;
            Vector2 vertical = Vector2.up * halfLen;
            Vector2 horizontal = Vector2.right * halfLen;

            var originalMatrix = Gizmos.matrix;

            if (zRotation != 0f) {
                Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, zRotation), Vector3.one);
            }

            DrawLine(center + vertical, center - vertical, color);
            DrawLine(center + horizontal, center - horizontal, color);


            Gizmos.matrix = originalMatrix;
        }

        public static void DrawArrow(Vector2 start, Vector2 end, Color color, float arrowLength, bool bReversed = false)
        {
            DrawLine(start, end, color);

            Vector2 lineDir;

            if (bReversed) {
                lineDir = (end - start).normalized;
                end -= lineDir;
            }

            else {
                lineDir = (start - end).normalized;
            }

            Vector2 perp = lineDir.Perpendicular();

            Vector2 betweenA = (lineDir + perp).normalized;
            Vector2 betweenB = Vector2.Reflect(-betweenA, lineDir);

            betweenA = (betweenA + lineDir).normalized;
            betweenB = (betweenB + lineDir).normalized;

            Vector2 arrowSideA = betweenA * arrowLength + end;
            Vector2 arrowSideB = betweenB * arrowLength + end;

            DrawLine(end, arrowSideA, color);
            DrawLine(end, arrowSideB, color);
        }

        public static void DrawRay(Vector2 start, Vector2 direction, float length, Color color)
        {
            var originColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawRay(start, direction * length);

            Gizmos.color = originColor;
        }
    }
}