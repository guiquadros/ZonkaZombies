using GlowingObjects.Scripts;
using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public abstract class InteractableGlowable : InteractableBase
    {
        [SerializeField, Tooltip("This is not obligatory")]
        private GlowableObject _glowableObject;

        public override void OnAwake()
        {
            base.OnAwake();

            if (_glowableObject != null)
            {
                _glowableObject.Glow(true);
            }
        }

        public override void OnSleep()
        {
            base.OnSleep();

            if (Count == 0)
            {
                if (_glowableObject != null)
                {
                    _glowableObject.Glow(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _glowableObject.Glow(true);
        }

        private void OnTriggerExit(Collider other)
        {
            _glowableObject.Glow(false);
        }
    }
}