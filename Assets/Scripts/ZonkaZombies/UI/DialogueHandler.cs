using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Input;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.UI
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _initializeDialogueOnStart = false;

        [SerializeField]
        private Dialogue _dialogue;

        public event Action DialogueStarted;
        public event Action DialogueFinished;

        private void Start()
        {
            if (_initializeDialogueOnStart)
            {
                StartDialogue();
            }
        }

        private void OnEnable()
        {
            DialogueManager.Instance.DialogueFinished += OnDialogueFinished;
        }

        private void OnDisable()
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

        private void OnDialogueFinished(Dialogue dialogue)
        {
            if (DialogueFinished != null && dialogue == _dialogue)
            {
                DialogueFinished();
            }
        }
    }
}
