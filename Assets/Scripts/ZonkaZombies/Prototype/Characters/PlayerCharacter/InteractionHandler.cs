﻿using UnityEngine;
using ZonkaZombies.Prototype.Scenery.Interaction;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class InteractionHandler : MonoBehaviour, IInteractor, ICommand
    {
        private IInteractable _interactable;
        
        private Player _characterBehaviour;

        internal void SetUp(Player abstractPlayerCharacterBehavior)
        {
            _characterBehaviour = abstractPlayerCharacterBehavior;
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
            if (interactable.GetGameObject() == _interactable.GetGameObject())
            {
                _interactable.OnSleep();
                _interactable = null;
            }
        }

        public void OnBegin()
        {
            _interactable.OnBegin(_characterBehaviour);
        }

        public Character GetCharacter()
        {
            return _characterBehaviour;
        }

        public void OnFinish() { }
    }
}