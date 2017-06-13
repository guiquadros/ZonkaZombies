using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralTakeDamageBehavior : BaseEnemyBehavior
    {
        [SerializeField]
        private ParticleSystem _damageParticleSystem;

        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.TakeDamage
            };
        }

        private void OnEnable()
        {
            if (!GenericEnemy.IsAlive)
            {
                ChangeBehavior(EEnemyBehavior.Die);
                return;
            }

            AudioManager.Instance.Play(GenericEnemy.EnemyDetails.GotHitClip);

            GenericEnemy.Animator.SetTrigger(EnemyAnimatorParameters.TAKE_DAMAGE);

            if (_damageParticleSystem != null)
            {
                _damageParticleSystem.Play();
            }

            ChangeBehavior(EEnemyBehavior.Pursuit);

        }
    }
}
