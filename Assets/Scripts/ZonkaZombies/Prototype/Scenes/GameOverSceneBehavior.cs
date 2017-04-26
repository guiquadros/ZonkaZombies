using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Scenes
{
    public class GameOverSceneBehavior : MonoBehaviour
    {
        private InputReader _inputReaderController1;
        private InputReader _inputReaderController2;

        private void Awake()
        {
            _inputReaderController1 = InputFactory.Create(InputType.Controller1);
            _inputReaderController2 = InputFactory.Create(InputType.Controller2);
        }

        private void Update()
        {
            if (_inputReaderController1.Start())
            {
                SceneManager.LoadScene(SceneConstants.P1_MANY_ENEMIES_VS_CHARACTER);
            }
            else if (_inputReaderController1.Back() || _inputReaderController2.Back())
            {
                SceneManager.LoadScene(SceneConstants.P2_MANY_ENEMIES_VS_CHARACTER);
            }
        }
    }
}
