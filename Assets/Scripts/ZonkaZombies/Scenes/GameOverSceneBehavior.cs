using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenes
{
    public class GameOverSceneBehavior : MonoBehaviour
    {
        [SerializeField, Range(0, 10)]
        private float _autoRestartDelay = 5f;

        private float _lastTime;

        private void Start()
        {
            _lastTime = Time.time;
        }

        private void Update()
        {
            if (PlayerInput.InputReaderController1.Start() || PlayerInput.InputReaderController2.Start() || Time.time - _lastTime >= _autoRestartDelay)
            {
                SceneManager.LoadScene(SceneConstants.PERSISTENT);
            }
        }
    }
}
