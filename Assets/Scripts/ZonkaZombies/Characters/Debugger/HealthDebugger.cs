using UnityEngine;

namespace ZonkaZombies.Characters.Debugger
{
    public class HealthDebugger : MonoBehaviour
    {
        private const string FormattedString = "{0} has taken <color=red>{1}</color> damage. New, its health is <color=gray>{2}</color>.";

        public Player.Behaviors.Player Player;

        private void Awake()
        {
            if (!Player)
                return;

            Player.Health.AddListener(OnHealthChanged);
        }

        private void OnDestroy()
        {
            if (!Player)
                return;

            Player.Health.RemoveListener(OnHealthChanged);
        }

        private void OnHealthChanged(int current, int damage, Character character)
        {
            UnityEngine.Debug.LogFormat(FormattedString, Player.gameObject.name, damage, current);
        }
    }
}