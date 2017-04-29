using UnityEngine;
using ZonkaZombies.Scenery.Interaction;
using ZonkaZombies.Util.Commands;

namespace ZonkaZombies.Characters.Player
{
    public class InteractionHandler : MonoBehaviour, IInteractor, ICommand
    {
        private IInteractable _interactable;
        
        private Player _player;

        public void SetUp(object obj)
        {
            _player = obj as Player;
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
            //TODO Review this!
            _interactable = interactable;

            _interactable.OnAwake();
        }

        public void OnExit(IInteractable interactable)
        {
            interactable.OnSleep();

            if (_interactable != null && interactable.GetGameObject() == _interactable.GetGameObject())
            {
                _interactable.OnFinish();
                _interactable = null;
            }
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
    }
}