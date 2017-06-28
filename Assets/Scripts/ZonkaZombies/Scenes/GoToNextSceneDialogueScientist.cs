using System;
using FreeAssets.Scripts;
using UnityEngine;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;
using ZonkaZombies.UI;
using ZonkaZombies.UI.Data;
using ZonkaZombies.UI.Dialogues;

namespace ZonkaZombies.Scenes
{
    public class GoToNextSceneDialogueScientist : MonoBehaviour
    {
        private bool _isReady = false;

        [SerializeField]
        private DialogueHandler _dialogueHandler;

        private void Start()
        {
            _dialogueHandler.DialogueFinished += OnDialogueFinished;
        }
        
        private void OnDestroy()
        {
            _dialogueHandler.DialogueFinished -= OnDialogueFinished;
        }

        private void OnDialogueFinished()
        {
            _isReady = true;
        }

        private void Update()
        {
            if (_isReady)
            {
                _isReady = false;
                SceneController.Instance.FadeAndLoadScene(GameScenes.CITY);
            }
        }
    }
}
