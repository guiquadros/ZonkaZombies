using System;
using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;

namespace Animations.Character
{
    public class GeneralStateMachineBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private EEnemyBehavior _enemyBehavior;

        public event Action<EEnemyBehavior> StateExit;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (StateExit != null)
            {
                StateExit(_enemyBehavior);
            }
        }
    }
}
