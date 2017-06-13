using UnityEngine;
using ZonkaZombies.Scenery.Interaction;

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

    // ReSharper disable once InconsistentNaming
    public static class IInteractableExtensions
    {
        public static bool SameAs(this IInteractable me, IInteractable other)
        {
            return me.GetGameObject() == other.GetGameObject();
        }
    }
}
