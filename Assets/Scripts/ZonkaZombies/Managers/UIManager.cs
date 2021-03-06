using System;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Characters;
using ZonkaZombies.Characters.Data.Stats;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Characters.Player.Weapon;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.UI;

namespace ZonkaZombies.Managers
{
    public class HudManager
    {
        //TODO
    }
    
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField]
        private GameUIManager _gameUiManager;

        private void OnEnable()
        {
            if (Instance != null)
            {
                // Remove duplicated UiManager GameObject
                Debug.LogWarning("Removing duplicated UiManager.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            MessageRouter.AddListener<OnPlayerHasBornMessage>(OnPlayerHasBornCallback);
            MessageRouter.AddListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
        }
        
        private void OnDisable()
        {
            MessageRouter.RemoveListener<OnPlayerHasBornMessage>(OnPlayerHasBornCallback);
            MessageRouter.RemoveListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
        }

        #region CALLBACKS - MESSAGE ROUTER

        private void OnPlayerHasBornCallback(OnPlayerHasBornMessage message)
        {
            _gameUiManager.AddPlayerHud(message.Player);
        }

        private void OnPlayerDeadCallback(OnPlayerDeadMessage message)
        {
            _gameUiManager.RemovePlayerHud(message.Player);
        }

        #endregion
    }
}
