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
            AudioManager.Instance.Play(GenericEnemy.EnemyDetails.GotHitClip);

            if (_damageParticleSystem != null)
            {
                _damageParticleSystem.Play();
            }

            if (!GenericEnemy.IsAlive)
            {
                ChangeBehavior(EEnemyBehavior.Die);
                return;
            }
            
            GenericEnemy.Animator.SetTrigger(EnemyAnimatorParameters.TAKE_DAMAGE);
            ChangeBehavior(EEnemyBehavior.Pursuit);

        }
    }
}
