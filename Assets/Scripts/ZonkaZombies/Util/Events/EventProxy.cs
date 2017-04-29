using UnityEngine;

namespace ZonkaZombies.Util.Events
{
    public sealed class EventProxy : MonoBehaviour
    {
        public static ListActionListener OnUpdate      = new ListActionListener();
        public static ListActionListener OnLateUpdate  = new ListActionListener();
        public static ListActionListener OnFixedUpdate = new ListActionListener();

        // This is used to make sure there is only one instance of EventProxy in the scene.
        private static EventProxy _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                if (_instance != this)
                {
                    Debug.LogError("Destroying invalid EventProxy reference.");
                    Destroy(gameObject);
                }
            }
            else
            {
                _instance = GetComponent<EventProxy>();
            }
        }

        private void Update()
        {
            OnUpdate.NotifyAll();
        }

        private void LateUpdate()
        {
            OnLateUpdate.NotifyAll();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate.NotifyAll();
        }
    }
}