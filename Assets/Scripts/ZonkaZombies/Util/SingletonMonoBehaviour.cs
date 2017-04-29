using UnityEngine;

namespace ZonkaZombies.Util
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = FindObjectOfType<T>();
        }
    }
}