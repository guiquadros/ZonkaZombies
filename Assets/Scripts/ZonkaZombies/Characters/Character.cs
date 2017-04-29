using UnityEngine;

namespace ZonkaZombies.Characters
{
    public abstract class Character : MonoBehaviour
    {
        public int LifePoints = 5;
        public int HitPoints = 1;

        public bool IsAlive { get; private set; }

        public delegate void OnCharacterDead(Character character);
        public OnCharacterDead OnDead;

        protected virtual void Start()
        {
            IsAlive = true;
        }

        /// <summary>
        /// Apply damage to the character.
        /// </summary>
        /// <param name="damage">Life Points the character lost.</param>
        /// <returns>Returns true if the character is dead.</returns>
        public void Damage(int damage)
        {
            Player.Player player = this as Player.Player;
            if (player != null && !player.CanReceiveDamage) return;

            LifePoints -= damage;

            IsAlive = LifePoints > 0;

            OnTakeDamage(damage);

            if (!IsAlive && OnDead != null)
            {
                OnDead(this);
            }
        }

        protected virtual void OnTakeDamage(int damage) { }
    }
}
