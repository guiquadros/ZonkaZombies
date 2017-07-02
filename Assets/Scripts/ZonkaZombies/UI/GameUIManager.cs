using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Characters.Player.Weapon;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.UI
{
    public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
    {
        [Header("HealthBars"), SerializeField]
        private PlayerHealthBar[] _playerHealthBar = new PlayerHealthBar[2];

        [Header("WeaponIcons")]
        public List<Image> weaponIcons;
        public List<Sprite> weaponIconSprites;

        [SerializeField]
        private Text _pressStartForPlayer2;

        [SerializeField]
        private Text _playerMissionCountText;

        public void UpdatePlayerMissionCountText(int count)
        {
            _playerMissionCountText.text = count.ToString();
        }

        private void OnEnable()
        {
            SceneController.Instance.OnSceneLoading += OnSceneLoading;
        }

        private void OnDisable()
        {
            SceneController.Instance.OnSceneLoading -= OnSceneLoading;
        }
        
        public void AddPlayerHud(Player player)
        {
            int index = GetPlayerIndex(player);
            _playerHealthBar[index].Initialize(player);

            //Setup player callbacks
            player.OnWeaponChanged += OnWeaponChangedCallback;

            //Initialize default weapon in hud
            OnWeaponChangedCallback(player.GetWeapon(), player);

            if (!player.IsFirstPlayer)
            {
                _pressStartForPlayer2.gameObject.SetActive(false);
            }
        }

        public void RemovePlayerHud(Player player)
        {
            int index = GetPlayerIndex(player);
            _playerHealthBar[index].Uninitialize(player);
        }

        #region PLAYER - CALLBACKS

        private void OnWeaponChangedCallback(WeaponDetails weapon, Player player)
        {
            int iconIndex = GetPlayerIndex(player);
            int weaponIndex = weapon == null ? (int) WeaponSubType.Punch : (int) weapon.WeaponSubType;
            weaponIcons[iconIndex].sprite = weaponIndex < 0 ? null : weaponIconSprites[weaponIndex];
        }

        private void OnSceneLoading(GameSceneType gameSceneType)
        {
            //force gameObject of second health bar to false when the first scene is loaded (new game started)
            if (gameSceneType.SceneName == GameScenes.GameScenesOrdered.First().SceneName && EntityManager.Instance.Players.Count <= 1 && _playerHealthBar.Length >= 2)
            {
                _playerHealthBar[1].gameObject.SetActive(false);
            }
        }

        #endregion

        #region HELPERS

        /// <summary>
        /// Perse from bool to an index between 0 and 1 that represent the choosen player. ZERO for player 1, and ONE for player 2.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private int GetPlayerIndex(Player player)
        {
            return player.IsFirstPlayer ? 0 : 1;
        }

        #endregion
    }
}
