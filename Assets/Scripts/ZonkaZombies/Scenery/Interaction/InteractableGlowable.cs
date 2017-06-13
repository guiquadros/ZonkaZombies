using GlowingObjects.Scripts;
using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public abstract class InteractableGlowable : InteractableBase
    {
        [SerializeField, Tooltip("This is not obligatory")]
        private GlowableObject _glowableObject;

        protected bool CanGlow = true;

        public override void OnAwake()
        {
            base.OnAwake();

            if (!CanGlow || _glowableObject.IsGlowing)
            {
                return;
            }

            if (_glowableObject != null)
            {
                _glowableObject.Glow(true);
            }
        }

        public override void OnSleep()
        {
            base.OnSleep();

            if (!CanGlow || !_glowableObject.IsGlowing)
            {
                return;
            }

            if (Count == 0)
            {
                if (_glowableObject != null)
                {
                    _glowableObject.Glow(false);
                }
            }
        }

        protected void StopGlowing()
        {
            CanGlow = false;
            SetGlowState(false);
        }

        protected void ContinueGlowing()
        {
            CanGlow = true;
            if (Count > 0)
            {
                SetGlowState(true);
            }
        }

        protected void ResetGlow()
        {
            CanGlow = true;
            SetGlowState(false);
        }

        private void SetGlowState(bool state)
        {
            _glowableObject.Glow(state);
        }
    }
}