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

        private void Start()
        {
            if (_initializeDialogueOnStart)
            {
                DialogueManager.Instance.Initialize(_dialogue, true);
            }
        }

        public void StartDialogue()
        {
            DialogueManager.Instance.Initialize(_dialogue);
        }
    }
}
