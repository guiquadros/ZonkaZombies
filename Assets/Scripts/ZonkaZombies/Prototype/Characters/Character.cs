using System;
using UnityEngine;
using ZonkaZombies.Prototype.UI;

namespace ZonkaZombies.Prototype.Characters
{
    public abstract class Character : MonoBehaviour
    {
        public int LifePoints = 5;
        public int HitPoints = 1;

        /// <summary>
        /// Apply damage to the character.
        /// </summary>
        /// <param name="damage">Life Points the character lost.</param>
        /// <param name="deathAction">Action to be executed when the character has no life points.</param>
        /// <returns>Returns true if the character is dead.</returns>
        public bool Damage(int damage, Action deathAction = null)
        {
            LifePoints -= damage;

            bool isDead = LifePoints <= 0;

            OnTakeDamage(damage);

            if (isDead && deathAction != null)
            {
                deathAction();
            }

            return isDead;
        }

        protected virtual void OnTakeDamage(int damage) { }
    }
}
