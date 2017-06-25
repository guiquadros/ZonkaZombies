using UnityEngine;
using ZonkaZombies.Managers;

namespace ZonkaZombies.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public void StartGameButton()
        {
            SceneController.Instance.LoadNextScene();
        }
    }
}
