using System.Collections.Generic;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralSleepingBehavior : BaseEnemyBehavior
    {
        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Sleep
            };
        }

        private void OnEnable()
        {
            GenericEnemy.Animator.SetBool(EnemyAnimatorParameters.IS_MOVING, false);
        }

        private void Update()
        {
            if (GenericEnemy.CanSeePlayerCharacter())
            {
                ChangeBehavior(EEnemyBehavior.Pursuit);
            }
        }

        private void OnDisable() { }
    }
}