using System;
using GlowingObjects.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private GameObject _exclamationMarkGameObject;

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
            _dialogueHandler.StartDialogue(transform);
        }

        public override void OnFinish(IInteractor interactor)
        {
            
        }

        private void DialogueHandler_OnDialogueFinished()
        {
            ExclamationMark(true);
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