using System.Collections.Generic;
using UnityEngine;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public abstract class BaseEnemyBehavior : MonoBehaviour
    {
        protected GenericEnemy GenericEnemy { get; private set; }
/// <summary>
        /// Is this enemy behavior already initialized with an instance to its Enemy?
        /// </summary>
        internal bool IsInitialized { get { return GenericEnemy != null; } }

        internal void Initialize(GenericEnemy genericEnemy)
        {
            GenericEnemy = genericEnemy;
        }

        /// <summary>
        /// Easier than calling by Enemy.TryChangeBehavior(...).
        /// </summary>
        /// <param name="behavior"></param>
        protected void ChangeBehavior(EEnemyBehavior behavior)
        {
            GenericEnemy.TryChangeBehavior(behavior);
        }

        public abstract List<EEnemyBehavior> GetValidStates();
    }
}