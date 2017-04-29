using UnityEngine;

namespace ZonkaZombies.Prototype.Characters.Enemy
{
    public class CrawlerEnemy : Enemy
    {
        [SerializeField, Range(.5f, 4)]
        private float _velocityMultiplier = 2;
        [SerializeField]
        private AnimationCurve _animationCurve;

        private float _agentMaximmumSpeed;

        protected override void OnPursuit()
        {
            base.OnPursuit();
            UpdateCrawlSpeed();
        }

        protected override void OnLoseSight()
        {
            base.OnLoseSight();
            UpdateCrawlSpeed();
        }

        protected override void OnPatrol()
        {
            base.OnPatrol();
            UpdateCrawlSpeed();
        }

        private void UpdateCrawlSpeed()
        {
            float currentDelta = Time.time * _velocityMultiplier;

            float currentAgentSpeed = Mathf.Sin(currentDelta);

            currentAgentSpeed = Mathf.Abs(currentAgentSpeed);

            // Dynamically sets the agent's speed based on the Animation Curve, to make the creeping effect
            Agent.speed = _animationCurve.Evaluate(currentAgentSpeed) * _agentMaximmumSpeed;
        }

        protected override void Start()
        {
            base.Start();

            _agentMaximmumSpeed = Agent.speed;
        }
    }
}
