using UnityEngine;
using ZonkaZombies.Prototype.Managers;

namespace ZonkaZombies.Prototype.Util
{
    public class TempCameraHandler : MonoBehaviour
    {
        private void Start()
        {
            SceneController sceneController = SceneController.Instance;
            sceneController.BeforeSceneUnload += Enable;
            sceneController.AfterSceneLoad    += Disable;
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}