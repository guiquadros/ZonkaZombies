using System;
using GlowingObjects.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Characters;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Managers;
using ZonkaZombies.UI;
using ZonkaZombies.UI.Dialogues;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenery.Interaction
{
    [RequireComponent(typeof(DialogueHandler))]
    public class DialogueInteractable : InteractableBase
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private GameObject _exclamationMarkGameObject;

        [SerializeField]
        private bool _interactOnlyOnce = false;

        private bool _alreadyInteracted = false;

        private DialogueHandler _dialogueHandler;

        private void Awake()
        {
            _dialogueHandler = GetComponent<DialogueHandler>();
        }

        private void OnEnable()
        {
            _dialogueHandler.DialogueStarted += DialogueHandler_OnDialogueStarted;
            _dialogueHandler.DialogueFinished += DialogueHandler_OnDialogueFinished;
        }

        private void OnDisable()
        {
            _dialogueHandler.DialogueStarted -= DialogueHandler_OnDialogueStarted;
            _dialogueHandler.DialogueFinished -= DialogueHandler_OnDialogueFinished;
        }

        public override void OnBegin(IInteractor interactor)
        {
            if (_interactOnlyOnce && _alreadyInteracted)
            {
                return;
            }

            Player player = interactor.GetCharacter() as Player;

            bool isPlayer2Dialogue = false;

            if (player != null)
            {
                isPlayer2Dialogue = !player.IsFirstPlayer;
            }

            _alreadyInteracted = true;
            _dialogueHandler.StartDialogue(transform, isPlayer2Dialogue);

            if (!GameManager.Instance.MissionCompleted)
            {
                _animator.SetTrigger(DialogueAnimatorParameters.TALK);
            }
        }

        public override void OnFinish(IInteractor interactor)
        {
            
        }

        private void DialogueHandler_OnDialogueFinished()
        {
            ExclamationMark(true);
            _animator.SetTrigger(DialogueAnimatorParameters.IDLE);
        }

        private void DialogueHandler_OnDialogueStarted()
        {
            ExclamationMark(false);
        }

        private void ExclamationMark(bool active)
        {
            if (_exclamationMarkGameObject != null)
            {
                _exclamationMarkGameObject.SetActive(active);
            }
        }
    }
}