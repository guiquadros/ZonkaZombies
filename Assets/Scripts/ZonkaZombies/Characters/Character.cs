using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZonkaZombies.Characters.Data.Stats;
using ZonkaZombies.Characters.Debugger;
using ZonkaZombies.Messaging;

#pragma warning disable 649

namespace ZonkaZombies.Characters
{
    [SuppressMessage("ReSharper", "DelegateSubtraction")]
    public abstract class Character : MonoBehaviour
    {
#region Editor Properties

        [SerializeField, Range(1, 200)]
        private int _healthPoints = 5;
        [SerializeField]
        private int _hitPoints = 1;

#endregion

        public BasicStat Health { get; private set; }
        public BasicStat Hit { get; private set; }

        public bool IsAlive
        {
            get { return Health.Current > 0; }
        }

        protected virtual void Awake()
        {
            Health = new BasicStat(_healthPoints, this);
            Hit = new BasicStat(_hitPoints, this);

#if STATS_DEBUGGER
            BasicStatDebugger.DebugStatsFrom(this);
#endif
        }

        /// <summary>
        /// Apply damage to the character.
        /// </summary>
        /// <param name="amount">Life Points the character lost.</param>
        /// <returns>Returns true if the character is dead.</returns>
        public void Damage(int amount)
        {
            Health.Remove(amount);
            OnDamaged();
        }

        public void Heal(int amount)
        {
            OnHealed();
            Health.Add(amount);
        }

        //TODO Move this method to another class, It does not belong this class..
        protected virtual void OnDamaged() { }

        protected virtual void OnHealed() { }

        protected virtual bool CanReceiveDamage()
        {
            return true;
        }

#region Callbacks

        #endregion

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}