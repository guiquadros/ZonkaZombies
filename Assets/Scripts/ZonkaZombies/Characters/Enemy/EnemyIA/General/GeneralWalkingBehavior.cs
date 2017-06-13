using System.Collections.Generic;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralWalkingBehavior : BaseEnemyBehavior
    {
        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Pursuit,
                EEnemyBehavior.Patrol,
                EEnemyBehavior.LoseSight
            };
        }

        private void OnEnable()
        {
            GenericEnemy.Animator.SetBool(EnemyAnimatorParameters.IS_MOVING, true);

            if (GenericEnemy.Agent.isOnNavMesh)
            {
                GenericEnemy.Agent.isStopped = false;
            }

            StartCoroutine(GenericEnemy.AutoGroom(.4f, .8f));
        }

        private void FixedUpdate()
        {
            if (GenericEnemy.HasTarget && GenericEnemy.Agent.isOnNavMesh)
            {
                GenericEnemy.FollowPlayerPosition();
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            if (GenericEnemy.Agent.isOnNavMesh)
            {
                GenericEnemy.Agent.isStopped = true;
            }
        }
    }
}