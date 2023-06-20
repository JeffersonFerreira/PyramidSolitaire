using UnityEngine;

namespace PyramidSolitaire.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 WithZ(this Vector2 vec, float z)
        {
            return WithZ((Vector3)vec, z);
        }

        public static Vector3 WithZ(this Vector3 vec, float z)
        {
            vec.z = z;
            return vec;
        }
    }
}