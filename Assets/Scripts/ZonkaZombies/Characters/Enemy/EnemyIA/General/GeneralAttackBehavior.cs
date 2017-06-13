using System;
using System.Collections.Generic;
using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralAttackBehavior : BaseEnemyBehavior
    {
        [SerializeField]
        private float _timeBetweenAttacks;

        private float _time = 0f;

        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Attack
            };
        }

        private void OnEnable()
        {
            GenericEnemy.EnemyTriggerExit += OnEnemyTriggerExit;
            Attack();
        }
        
        private void Update()
        {
            _time += Time.deltaTime;

            if (_time >= _timeBetweenAttacks)
            {
                Attack();
                _time = 0f;
            }
        }

        private void OnDisable()
        {
            if (GenericEnemy != null)
            {
                GenericEnemy.EnemyTriggerExit -= OnEnemyTriggerExit;
            }
        }

        private void OnEnemyTriggerExit(Collider other) 
        {
            if (other.CompareTag(TagConstants.PLAYER))
            {
                ChangeBehavior(EEnemyBehavior.Pursuit);
            }
        }

        private void Attack()
        {
            GenericEnemy.Animator.SetTrigger(EnemyAnimatorParameters.ATTACK);
        }
    }
}