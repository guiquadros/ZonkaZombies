using UnityEngine;
using ZonkaZombies.Managers;

namespace ZonkaZombies.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        private void Awake()
        {
            SceneController.Instance.CurrentSceneIndex = 0;
        }

        public void StartGameButton()
        {
            SceneController.Instance.LoadNextScene();
        }
    }
}
