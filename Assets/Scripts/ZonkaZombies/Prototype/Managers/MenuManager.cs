using UnityEngine;

namespace ZonkaZombies.Prototype.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void StartSingleplayerMode()
        {
            StartNewGame(GameModeType.Singleplayer);
        }

        public void StartMultiplayerMode()
        {
            StartNewGame(GameModeType.Multiplayer);
        }

        private void StartNewGame(GameModeType gameType)
        {
            GameManager.Instance.GameMode = gameType;
            SceneController.Instance.LoadNextScene();
        }
    }
}
