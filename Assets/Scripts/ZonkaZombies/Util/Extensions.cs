using UnityEngine;

namespace ZonkaZombies.Util
{
    public static class Vector3Extensions
    {
        public static Vector3 MultiplyBy(this Vector3 value, Vector3 other)
        {
            value.x *= other.x;
            value.y *= other.y;
            value.z *= other.z;
            return value;
        }
    }
}
