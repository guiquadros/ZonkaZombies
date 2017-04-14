using UnityEngine;
using ZonkaZombies.Prototype.Scenery.Interaction;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class InteractionHandler : MonoBehaviour
    {
        private AbstractInteractable _interactable;
        public bool IsInteractingWithSomething { get; private set; }
        
        private PlayerCharacterBehavior _playerCharacterBehavior;

        [SerializeField]
        private Transform _characterHand;

        internal void SetUp(PlayerCharacterBehavior playerCharacterBehavior)
        {
            _playerCharacterBehavior = playerCharacterBehavior;
        }

        protected virtual void Update()
        {
            if (IsInteractingWithSomething)
            {
                HandleInteraction();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            // If we are already interacting with something, do nothing
            if (IsInteractingWithSomething)
            {
                return;
            }

            AbstractInteractable tempInteractable = other.gameObject.GetComponent<AbstractInteractable>();

            if (tempInteractable != null && !tempInteractable.IsSomeoneInteractingWithIt && tempInteractable.IsValidForInteraction(_playerCharacterBehavior.Type))
            {
                _interactable = tempInteractable;

                // TODO Show hud info
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!IsInteractingWithSomething || other.gameObject != _interactable.gameObject)
            {
                // Make sure the _interactable is null
                _interactable = null;
                return;
            }

            FinishCurrentInteraction();
        }

        private void FinishCurrentInteraction()
        {
            _interactable.FinishInteraction();
            _interactable = null;
            IsInteractingWithSomething = false;
            // TODO Hude hud info
        }

        private void HandleInteraction()
        {
            // TODO Handle interaction logic. It could be better to break the logic into different classes, so each one handles its own logic (for example, per object type). We can handle player input in here, too!

            _interactable.MainGameObject.transform.position = _characterHand.position; //Holds the object in character hand
        }

        internal void TryToInteract()
        {
            if (_interactable != null)
            {
                if (IsInteractingWithSomething)
                {
                    FinishCurrentInteraction();
                }
                else
                {
                    IsInteractingWithSomething = true;
                    _interactable.StartInteraction(_playerCharacterBehavior);
                }
            }
        }
    }
}