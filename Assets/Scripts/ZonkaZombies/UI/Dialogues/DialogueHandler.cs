using System;
using UnityEngine;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.UI.Dialogues
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _initializeDialogueOnStart = false;

        [SerializeField]
        private Dialogue _dialogue;

        public event Action DialogueStarted;
        public event Action DialogueFinished;

        public Dialogue Dialogue
        {
            get { return _dialogue; }
        }

        private void Start()
        {
            if (_initializeDialogueOnStart)
            {
                StartDialogue();
            }
        }

        protected virtual void OnEnable()
        {
            DialogueManager.Instance.DialogueFinished += OnDialogueFinished;
        }

        protected virtual void OnDisable()
        {
            DialogueManager.Instance.DialogueFinished -= OnDialogueFinished;
        }

        public void StartDialogue(Transform interactableTransform = null)
        {
            if (DialogueStarted != null)
            {
                DialogueStarted();
            }
            
            DialogueManager.Instance.Initialize(_dialogue, _initializeDialogueOnStart, interactableTransform);
        }

        private void OnDialogueFinished(Dialogue dialogue, bool freezePlayer)
        {
            if (DialogueFinished != null && dialogue == _dialogue)
            {
                DialogueFinished();
            }
        }
    }
}
