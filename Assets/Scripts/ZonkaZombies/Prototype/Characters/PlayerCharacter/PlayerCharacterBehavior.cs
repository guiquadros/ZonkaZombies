using UnityEngine;
using ZonkaZombies.Input;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class PlayerCharacterBehavior : CharacterBehavior
    {
        public InputReader InputReader;
        
        [SerializeField]
        protected Animator Animator;
        
        private static readonly int ANIM_PUNCH_ID = Animator.StringToHash("Punch");

        protected virtual void Awake()
        {
            if (!Animator)
            {
                Debug.LogError("Animator component cannot be null!");
            }
        }

        protected void DoPunch()
        {
            Animator.SetTrigger(ANIM_PUNCH_ID);
        }
    }
}