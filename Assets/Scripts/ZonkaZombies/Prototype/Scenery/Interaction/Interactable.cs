using GlowingObjects.Scripts;
using UnityEngine;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField, Tooltip("This is not obligatory")]
        private GlowableObject _glowableObject;
        private int _playersInteractingCount;

        public virtual void OnAwake()
        {
            _playersInteractingCount++;

            if (_glowableObject != null)
            {
                _glowableObject.Glow(true);
            }
        }

        public virtual void OnSleep()
        {
            _playersInteractingCount--;

            _playersInteractingCount = Mathf.Max(_playersInteractingCount, 0);

            if (_playersInteractingCount == 0)
            {
                if (_glowableObject != null)
                {
                    _glowableObject.Glow(false);
                }
            }
        }

        public abstract void OnBegin(IInteractor interactor);
        public abstract void OnFinish();

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}