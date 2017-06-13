using UnityEngine;
using ZonkaZombies.Scenery.Interaction;
using ZonkaZombies.Util;
using ZonkaZombies.Util.Commands;

namespace ZonkaZombies.Characters.Player.Interaction
{
    public class InteractionHandler : MonoBehaviour, IInteractor, ICommand
    {
        private IInteractable _interactable;

        public bool CanDiscardWeapon
        {
            get
            {
                return _interactable == null || (_interactable as DoorInteractable) == null;
            }
        }

        private Behaviors.Player _player;

        public void SetUp(object obj)
        {
            _player = obj as Behaviors.Player;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

            if (interactable == null)
            {
                return;
            }

            OnEnter(interactable);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

            if (interactable == null)
            {
                return;
            }

            OnExit(interactable);            
        }

        /// <summary>
        /// This method is called when the player presses the ACTION button.
        /// </summary>
        public void Execute()
        {
            if (_interactable == null)
            {
                return;
            }

            OnBegin();
        }

        public void OnEnter(IInteractable interactable)
        {
            // If we are already interacting with something, do nothing
            if (_interactable != null)
            {
                OnExit(_interactable);
            }

            _interactable = interactable;

            _interactable.OnAwake();
        }

        public void OnExit(IInteractable interactable)
        {
            if (_interactable != null)
            {
                if (interactable.SameAs(_interactable))
                {
                    _interactable.OnFinish(this);
                    _interactable = null;
                }
            }

            interactable.OnSleep();
        }

        public void OnBegin()
        {
            _interactable.OnBegin(this);
        }

        public Character GetCharacter()
        {
            return _player;
        }

        public void OnFinish() { }

        public void ForceFinish()
        {
            _interactable = null;
        }
    }
}