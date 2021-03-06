using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Managers;

namespace ZonkaZombies.UI
{
    public class EnableHUD : MonoBehaviour
    {
        [SerializeField]
        private Text _pressStartForPlayer2;

        [SerializeField]
        private GameObject _playerChocolateMissionCount;

        [SerializeField]
        private GameObject _hallFirstFloorMissions;

        public void OnEnable()
        {
            _pressStartForPlayer2.gameObject.SetActive(true);
        }

        private void Start()
        {
            gameObject.SetActive(false);
            SceneController.Instance.OnSceneLoading += OnSceneLoading;
        }

        private void OnDestroy()
        {
            SceneController.Instance.OnSceneLoading -= OnSceneLoading;
        }

        private void OnSceneLoading(GameSceneType gameScene)
        {
            gameObject.SetActive(gameScene.ShowHud);
            _pressStartForPlayer2.gameObject.SetActive(gameScene.ShowPressStart);
            _playerChocolateMissionCount.SetActive(gameScene.ShowChocolateMissionCount);
            _hallFirstFloorMissions.SetActive(gameScene.ShowHallFirstFloorMissions);
        }
    }
}
