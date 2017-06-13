using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Characters.Player.Weapon;

namespace ZonkaZombies.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("HealthBars"), SerializeField]
        private PlayerHealthBar[] _playerHealthBar = new PlayerHealthBar[2];

        [Header("WeaponIcons")]
        public List<Image> weaponIcons;
        public List<Sprite> weaponIconSprites;

        [SerializeField]
        private Text _pressStartForPlayer2;

        public void Awake()
        {
            _pressStartForPlayer2.gameObject.SetActive(true);
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

#endregion

#region HELPERS

       /// <summary>
        /// Perse from bool to an index between 0 and 1 that represent the choosen player. ZERO for player 1, and ONE for player 2.
        /// </summary>
        /// <param name="isFirstPlayer"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int GetPlayerIndex(Player player)
        {
            return player.IsFirstPlayer ? 0 : 1;
        }

#endregion
    }
}
