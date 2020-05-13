
using UnityEngine;

namespace AutonomousAgents
{

    public static class ExtensionMethods
    {

        #region Vector2 Extensions

        /// <summary>
        /// Test if the vector is (0, 0).
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsZero(this Vector2 v)
        {
            return v == Vector2.zero;
        }

        /// <summary>
        /// Test if the vector is close to (0, 0) within epsilon tolerance.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool NearZero(this Vector2 v, float epsilon = 0.001f)
        {
            return v.sqrMagnitude <= epsilon * epsilon;
        }

        /// <summary>
        /// Get the perpendicular vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(v.y, -v.x).normalized;
        }

        /// <summary>
        /// Tests if two vectors have almost equal normal vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualNormals(this Vector2 a, Vector2 b)
        {
            a.Normalize();
            b.Normalize();

            return NormalizedEqual(a, b);
        }

        /// <summary>
        /// Tests if the normals vectors are the same.
        /// NOTE: Use this function on vectors that are already normalized.
        /// </summary>
        /// <param name="normalA"></param>
        /// <param name="normalB"></param>
        /// <returns></returns>
        public static bool NormalizedEqual(this Vector2 normalA, Vector2 normalB)
        {
            float dot = Vector2.Dot(normalA, normalB);

            // The two vectors are almost equal directions 
            // if their dot product is near 1.
            return Mathf.Abs(1f - dot) < 0.000001f;
        }

        /// <summary>
        /// Rotate a vector by the specified degrees.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            // The angle of the vector we want to rotate.
            float originalAngle = Mathf.Atan2(v.y, v.x);

            // The amount we want to rotate the vector.
            float deltaAngle = Mathf.Deg2Rad * degrees;

            // The new absolute angle of the rotated vector.
            float totalRadians = originalAngle + deltaAngle;

            // The components of the new rotated vector.
            float x = Mathf.Cos(totalRadians);
            float y = Mathf.Sin(totalRadians);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a new vector (abs(x), abs(y)).
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        /// <summary>
        /// Checks if vectors a and b are almost equal within epsilon tolerance.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool AlmostEqual(this Vector2 a, Vector2 b, float epsilon = 0.001f)
        {
            return NearZero((a - b).Abs(), epsilon);
        }

        /// <summary>
        /// Tests if the distance between vectors A and B is less than the distance specified.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="distanceSq"></param>
        /// <returns></returns>
        public static bool WithinDistanceSq(this Vector2 a, Vector2 b, float distanceSq)
        {
            Vector2 toA = a - b;
            float sq = toA.sqrMagnitude;
            return sq < distanceSq;
        }

        #endregion

        #region Transform Extensions

        /// <summary>
        /// Get the transform position as a 2D vector.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 Position2D(this Transform value)
        {
            return (Vector2)value.position;
        }

        /// <summary>
        /// Get the transform rotation as a 2D vector.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Orientation(this Transform t)
        {
            return t.rotation * Vector3.up;
        }

        #endregion

        #region Collider2D Extensions

        public static float GetBoundingRadius(this Collider2D collider)
        {
            Vector2 extents = collider.bounds.extents;
            return Mathf.Max(extents.x, extents.y);
        }

        #endregion

        #region String Extensions

        /// <summary>
        /// Returns true if the source string starts with the prefix.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool HasPrefix(this string source, string prefix)
        {
            // Prefix string larger than source string.
            if (source.Length < prefix.Length) {
                return false;
            }

            // Iterate through the prefix string to test matching characters.
            for (int i = 0; i < prefix.Length; ++i) {

                // Mismatch occured.
                if (prefix[i] != source[i]) {
                    return false;
                }
            }

            // All prefix characters matched in the source string.
            return true;
        }

        #endregion
    }
}
