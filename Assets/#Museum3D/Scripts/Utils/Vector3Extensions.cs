using UnityEngine;

namespace Mus3d
{
    public static class Vector3Extensions
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithX (this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithY (this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithZ (this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithXY (this Vector3 vector, float x, float y)
        {
            vector.x = x;
            vector.y = y;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithXZ (this Vector3 vector, float x, float z)
        {
            vector.x = x;
            vector.z = z;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 WithYZ (this Vector3 vector, float y, float z)
        {
            vector.y = y;
            vector.z = z;
            return vector;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Vector3 RotateOnXZPlane (this Vector3 vector, float angle_deg)
        {
            float x = vector.x * Mathf.Cos (angle_deg * Mathf.Deg2Rad) - vector.z * Mathf.Sin (angle_deg * Mathf.Deg2Rad);
            float z = vector.x * Mathf.Sin (angle_deg * Mathf.Deg2Rad) + vector.z * Mathf.Cos (angle_deg * Mathf.Deg2Rad);

            return vector.WithXZ (x, z);
        }
    }
}