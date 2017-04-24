using UnityEngine;

namespace ZonkaZombies.Util
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            Instance = FindObjectOfType<T>();
        }
    }
}