using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Characters.Player;

namespace ZonkaZombies.UI
{
    public class PlayerLifePointsBehavior : MonoBehaviour
    {
        [SerializeField]
        private Player _player;

        [SerializeField]
        private Text _lifePointsText;

        [SerializeField, Tooltip("The pattern to be used in the Text value")]
        private string _pattern;

        private void Start()
        {
            if (string.IsNullOrEmpty(_pattern))
            {
                _pattern = "{0}";
            }
        }

        private void Update()
        {
            _lifePointsText.text = string.Format(_pattern, _player.LifePoints.ToString("00"));
        }
    }
}
