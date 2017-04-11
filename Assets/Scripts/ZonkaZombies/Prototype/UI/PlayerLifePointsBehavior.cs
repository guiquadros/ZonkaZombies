using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;

namespace ZonkaZombies.Prototype.UI
{
    public class PlayerLifePointsBehavior : MonoBehaviour
    {
        [SerializeField]
        private PlayerCharacterBehavior _playerCharacterBehavior;

        [SerializeField]
        private Text _lifePointsText;

        private void Update()
        {
            _lifePointsText.text = _playerCharacterBehavior.LifePoints.ToString("00");
        }
    }
}
