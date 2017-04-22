using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;

namespace ZonkaZombies.Prototype.UI
{
    public class PlayerLifePointsBehavior : MonoBehaviour
    {
        [SerializeField]
        private Player _abstractPlayerCharacterBehavior;

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
            _lifePointsText.text = string.Format(_pattern, _abstractPlayerCharacterBehavior.LifePoints.ToString("00"));
        }
    }
}
