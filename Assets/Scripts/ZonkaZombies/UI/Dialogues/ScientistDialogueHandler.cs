using System;
using UnityEngine;
using ZonkaZombies.Managers;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.UI.Dialogues
{
    public class ScientistDialogueHandler : DialogueHandler
    {
        [SerializeField]
        private Dialogue _finalScientistDialogue;

        private void Awake()
        {
            if (_finalScientistDialogue != null && GameManager.Instance.MissionCompleted)
            {
                _dialogue = _finalScientistDialogue;
            }
        }
    }
}
