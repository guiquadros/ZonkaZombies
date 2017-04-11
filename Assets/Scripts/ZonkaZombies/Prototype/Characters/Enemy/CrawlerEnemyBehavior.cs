using UnityEngine;

namespace ZonkaZombies.Prototype.Characters.Enemy
{
    public class CrawlerEnemyBehavior : EnemyBehavior
    {
        [SerializeField, Range(.5f, 4)]
        private float _velocityMultiplier = 2;
        [SerializeField]
        private AnimationCurve _animationCurve;

        private float _agentMaximmumSpeed;

        private void Start()
        {
            _agentMaximmumSpeed = agent.speed;
        }

        protected override void Update()
        {
            float currentDelta = Time.time * _velocityMultiplier;

            float currentAgentSpeed = Mathf.Sin(currentDelta);

            currentAgentSpeed = Mathf.Abs(currentAgentSpeed);

            // Dynamically sets the agent's speed based on the Animation Curve, to make the creeping effect
            agent.speed = _animationCurve.Evaluate(currentAgentSpeed) * _agentMaximmumSpeed;

            base.Update();
        }
    }
}
