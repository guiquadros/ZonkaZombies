using System;
using System.Linq;
using FreeAssets.Scripts;
using UnityEngine;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;
using ZonkaZombies.UI;
using ZonkaZombies.UI.Data;
using ZonkaZombies.UI.Dialogues;

namespace ZonkaZombies.Scenes
{
    public class GoToNextSceneIntro : MonoBehaviour
    {
        [SerializeField]
        private string _sceneToLoad;

        [SerializeField]
        private bool _skipNextSceneWhenPressA = false;

        [SerializeField]
        private bool _forceLoadSceneParameter = false;

        private bool _isReady = false;
        //private bool _sceneAlreadyLoaded = false;

        private void Start()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.DialogueFinished += OnDialogueFinished;
            }

            //if (RetroPrinterScriptBasic.Instance != null)
            //{
            //    RetroPrinterScriptBasic.Instance.KeyboardAnimationFinished += OnKeyboardAnimationFinished;
            //}
        }
        
        private void OnDisable()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.DialogueFinished -= OnDialogueFinished;
            }

            //if (RetroPrinterScriptBasic.Instance != null)
            //{
            //    RetroPrinterScriptBasic.Instance.KeyboardAnimationFinished -= OnKeyboardAnimationFinished;
            //}
        }

        private void OnDialogueFinished(Dialogue dialogue, bool freezePlayer)
        {
            _isReady = true;
        }

        //private void OnKeyboardAnimationFinished()
        //{
        //    _isReady = true;
        //}

        private void Update()
        {
            GameSceneType scene = GameScenes.GameScenesOrdered.First(gso => gso.SceneName == _sceneToLoad);

            if (_skipNextSceneWhenPressA && /*!_sceneAlreadyLoaded &&*/ (PlayerInput.InputReaderController1.ADown() || PlayerInput.InputReaderController2.ADown() ||
                PlayerInput.InputReaderKeyboard.ADown()))
            {
                if (_forceLoadSceneParameter)
                {
                    SceneController.Instance.FadeAndLoadScene(scene);
                }
                else
                {
                    //_sceneAlreadyLoaded = true;
                    //SceneController.Instance.FadeAndLoadScene(scene);
                    SceneController.Instance.LoadNextScene();
                }
            }
            else if (_isReady)
            {
                _isReady = false;

                if (_forceLoadSceneParameter)
                {
                    SceneController.Instance.FadeAndLoadScene(scene);
                }
                else
                {
                    //SceneController.Instance.FadeAndLoadScene(scene);
                    SceneController.Instance.LoadNextScene();
                }
            }
        }
    }
}
