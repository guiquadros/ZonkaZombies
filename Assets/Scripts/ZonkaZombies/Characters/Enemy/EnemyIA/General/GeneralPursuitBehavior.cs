using System.Collections.Generic;
using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.General
{
    public class GeneralPursuitBehavior : BaseEnemyBehavior
    {
        [SerializeField]
        private float _pursuitTimeout = 5f;

        private float _time = 0f;

        public override List<EEnemyBehavior> GetValidStates()
        {
            return new List<EEnemyBehavior>
            {
                EEnemyBehavior.Pursuit
            };
        }

        private void OnEnable()
        {
            GenericEnemy.EnemyTriggerEnter += OnEnemyTriggerEnter;
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if (!GenericEnemy.HasTarget)
            {
                GenericEnemy.FindNearestPlayer();
                return;
            }

            if (!GenericEnemy.CanSeePlayerCharacter() && _time > _pursuitTimeout)
            {
                ChangeBehavior(EEnemyBehavior.LoseSight);
            }

            if (_time > _pursuitTimeout)
            {
                _time = 0f;
            }
        }

        private void OnDisable()
        {
            GenericEnemy.EnemyTriggerEnter -= OnEnemyTriggerEnter;
        }

        private void OnEnemyTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagConstants.PLAYER))
            {
                Debug.Log("OnEnemyTriggerEnte - PLAYER");
                ChangeBehavior(EEnemyBehavior.Attack);
            }
        }
    }
}
