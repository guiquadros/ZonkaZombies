using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZonkaZombies.Input;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private InputType _inputType = InputType.Controller1;

        [SerializeField]
        private Image _singleplayerSelectionImage, _multiplayerSelectionImage; 

        private InputReader _inputReader;

        private void Awake()
        {
            _inputReader = InputFactory.Create(_inputType);
        }

        public void Update()
        {
            if (_inputReader.DigitalPadHorizontal() < 0)
            {
                _singleplayerSelectionImage.enabled = true;
                _multiplayerSelectionImage.enabled = false;
            }

            if (_inputReader.DigitalPadHorizontal() > 0)
            {
                _singleplayerSelectionImage.enabled = false;
                _multiplayerSelectionImage.enabled = true;
            }

            if (_inputReader.ADown())
            {
                GameManager.Instance.GameMode = _multiplayerSelectionImage.enabled
                    ? GameModeType.Multiplayer
                    : GameModeType.Singleplayer;

                SceneController.Instance.LoadNextScene();
            }
        }
    }
}
