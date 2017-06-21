using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenes
{
    public class GameOverSceneBehavior : MonoBehaviour
    {
        [SerializeField, Range(0, 10)]
        private float _autoRestartDelay = 5f;

        private float _lastTime;

        private bool _alreadyLoaded = false;
        
        private void Start()
        {
            _lastTime = Time.time;
            _alreadyLoaded = false;
        }

        private void Update()
        {
            if ((PlayerInput.InputReaderController1.Start() || PlayerInput.InputReaderController2.Start() || Time.time - _lastTime >= _autoRestartDelay) && !_alreadyLoaded)
            {
                _alreadyLoaded = true;
                SceneController.Instance.CurrentSceneIndex = 0;
                SceneController.Instance.FadeAndLoadScene(GameScenes.GameScenesOrdered.First());
            }
        }
    }
}
