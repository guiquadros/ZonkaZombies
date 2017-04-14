using System;
using UnityEngine;

namespace ZonkaZombies.Prototype.Characters
{
    public abstract class CharacterBehavior : MonoBehaviour
    {
        public int LifePoints = 5;
        public int HitPoints = 1;

        /// <summary>
        /// Apply damage to the character.
        /// </summary>
        /// <param name="lifePoints">Life Points the character lost.</param>
        /// <param name="deathAction">Action to be executed when the character has no life points.</param>
        /// <returns>Returns true if the character is dead.</returns>
        public bool Damage(int lifePoints, Action deathAction = null)
        {
            LifePoints -= lifePoints;

            bool isDead = LifePoints <= 0;

            if (isDead && deathAction != null)
            {
                deathAction();
            }

            return isDead;
        }
    }
}
