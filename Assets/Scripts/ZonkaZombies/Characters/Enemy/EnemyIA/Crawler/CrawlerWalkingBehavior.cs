using System.Collections.Generic;
using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA.General;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.Crawler
{
    public class CrawlerWalkingBehavior : BaseEnemyBehavior
    {
        [SerializeField, Range(.5f, 20)]
        private float _velocityMultiplier = 2;
        [SerializeField]
        private AnimationCurve _animationCurve;

        [SerializeField]
        private float _agentMaximmumSpeed;

        protected void UpdateCrawlSpeed()
        {
            float currentDelta = Time.time * _velocityMultiplier;

            float currentAgentSpeed = Mathf.Sin(currentDelta);

            currentAgentSpeed = Mathf.Abs(currentAgentSpeed);

            // Dynamically sets the agent's speed based on the Animation Curve, to make the creeping effect
            GenericEnemy.Agent.speed = _animationCurve.Evaluate(currentAgentSpeed) * _agentMaximmumSpeed;
        }

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

            if (GenericEnemy.Agent.isActiveAndEnabled)
            {
                GenericEnemy.Agent.isStopped = false;
            }
        }

        private void Update()
        {
            if (GenericEnemy.HasTarget)
            {
                UpdateCrawlSpeed();
            }
        }

        private void OnDisable()
        {
            if (GenericEnemy.Agent.isOnNavMesh)
            {
                GenericEnemy.Agent.isStopped = true;
            }
        }
    }
}
