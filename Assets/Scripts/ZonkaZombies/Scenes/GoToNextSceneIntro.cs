using System;
using FreeAssets.Scripts;
using UnityEngine;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;
using ZonkaZombies.UI;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.Scenes
{
    public class GoToNextSceneIntro : MonoBehaviour
    {
        private bool _isReady = false;

        private void Start()
        {
            DialogueManager.Instance.DialogueFinished += OnDialogueFinished;

            if (RetroPrinterScriptBasic.Instance != null)
            {
                RetroPrinterScriptBasic.Instance.KeyboardAnimationFinished += OnKeyboardAnimationFinished;
            }
        }
        
        private void OnDisable()
        {
            DialogueManager.Instance.DialogueFinished -= OnDialogueFinished;

            if (RetroPrinterScriptBasic.Instance != null)
            {
                RetroPrinterScriptBasic.Instance.KeyboardAnimationFinished -= OnKeyboardAnimationFinished;
            }
        }

        private void OnDialogueFinished(Dialogue dialogue)
        {
            _isReady = true;
        }

        private void OnKeyboardAnimationFinished()
        {
            _isReady = true;
        }

        private void Update()
        {
            if (_isReady)
            {
                _isReady = false;
                SceneController.Instance.LoadNextScene();
            }
        }
    }
}
