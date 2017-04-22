using UnityEngine;

namespace ZonkaZombies.Util
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; protected set; }

        private void Awake()
        {
            Instance = FindObjectOfType<T>();
        }
    }
}