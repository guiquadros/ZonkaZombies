using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralDeathBehavior : BaseEnemyBehavior
    {
        [SerializeField]
        private float _deathSpeed = 1.0f;

        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Die
            };
        }

        private void OnEnable()
        {
            GenericEnemy.Animator.SetFloat(EnemyAnimatorParameters.DEATH_SPEED, _deathSpeed);
            GenericEnemy.Animator.SetBool(EnemyAnimatorParameters.IS_DEAD, true);
        }
    }
}
