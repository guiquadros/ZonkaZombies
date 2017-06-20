using GlowingObjects.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenery.Interaction
{
    public class ElevatorFloorInteractable : InteractableBase
    {
        private bool _alreadyInteracted = false;

        private void OnEnable()
        {
            SceneController.Instance.AfterSceneLoad += OnAfterSceneLoad;
        }

        private void OnDisable()
        {
            SceneController.Instance.AfterSceneLoad -= OnAfterSceneLoad;
        }

        private void OnAfterSceneLoad()
        {
            _alreadyInteracted = false;
        }

        public override void OnBegin(IInteractor interactor)
        {
            //if all players are inside the elevator and all enemies were killed
            if (Count == EntityManager.Instance.Players.Count && EntityManager.Instance.Enemies.Count <= 0 && !_alreadyInteracted)
            {
                _alreadyInteracted = true;
                SceneController.Instance.LoadNextScene();
            }
        }

        public override void OnFinish(IInteractor interactor)
        {
            
        }
    }
}