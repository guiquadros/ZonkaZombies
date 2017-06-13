using System.Collections.Generic;
using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralLoseSight : BaseEnemyBehavior
    {
        [SerializeField, Range(0, 10, order = 1)]
        private float _sleepTimer;

        private float _losePlayerStartTimer;

        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.LoseSight
            };
        }

        private void OnEnable()
        {
            GenericEnemy.Animator.SetBool(EnemyAnimatorParameters.IS_MOVING, true);
            _losePlayerStartTimer = Time.time;
        }

        private void Update()
        {
            float timerDiff = Time.time - _losePlayerStartTimer;
            if (timerDiff >= _sleepTimer || !GenericEnemy.HasTarget)
            {
                ChangeBehavior(EEnemyBehavior.Sleep);
            }
            else
            {
                if (GenericEnemy.CanSeePlayerCharacter())
                {
                    ChangeBehavior(EEnemyBehavior.Pursuit);
                }
            }
        }

        private void OnDisable()
        {
            
        }
    }
}
