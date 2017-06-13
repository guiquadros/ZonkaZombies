using System.Text;
using UnityEngine;

namespace ZonkaZombies.Characters.Debugger
{
    public static class BasicStatDebugger
    {
        public static void DebugStatsFrom(Character character)
        {
            character.Health.AddListener(DebugHealthStatCallback);
        }

        private static void DebugHealthStatCallback(int current, int damage, Character character)
        {
            const string damageStringFormat = "OnTakeDamage [current:{0} | damage:{1} | character:{2}]";
            Debug.Log(string.Format(damageStringFormat, current, damage, character.ToString()));
        }
    }
}