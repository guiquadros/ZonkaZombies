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

        [SerializeField, Range(0, 10)]
        private float _autoRestartDelay = 5f;

        private float _lastTime;

        private void Awake()
        {
            _inputReaderController1 = InputFactory.Create(InputType.Controller1);
            _inputReaderController2 = InputFactory.Create(InputType.Controller2);
        }

        private void Start()
        {
            _lastTime = Time.time;
        }

        private void Update()
        {
            if (_inputReaderController1.Back() || _inputReaderController2.Back())
            {
                SceneManager.LoadScene(SceneConstants.P2_MANY_ENEMIES_VS_CHARACTER);
            }

            if (_inputReaderController1.Start() || _inputReaderController2.Start() || Time.time - _lastTime >= _autoRestartDelay)
            {
                SceneManager.LoadScene(SceneConstants.PERSISTENT);
            }
        }
    }
}
