using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralIdleBehavior : BaseEnemyBehavior
    {
        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Idle
            };
        }

        private SphereCollider _triggerCollider;

        private void Awake()
        {
            _triggerCollider = GetComponents<SphereCollider>().ToList().Find(c => c.isTrigger);
        }

        private void OnEnable()
        {
            GenericEnemy.Animator.SetBool(EnemyAnimatorParameters.IS_MOVING, false);
            GenericEnemy.EnemyTriggerEnter += OnEnemyTriggerEnter;
            _triggerCollider.enabled = true;

            StartCoroutine(GenericEnemy.AutoGroom(.1f, .6f));
        }

        private void OnEnemyTriggerEnter(Collider obj)
        {
            if (obj.CompareTag(TagConstants.PLAYER))
            {
                ChangeBehavior(EEnemyBehavior.Sleep);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _triggerCollider.enabled = false;
        }
    }
}
