using Assets.Scripts.Character.Attribute;
using UnityEngine;

namespace Assets.Scripts.Character
{
    [RequireComponent(typeof(CharacterMovementController))]
    public class CharacterBehaviour : MonoBehaviour
    {
        public delegate void OnDie();

        private CharacterMovementController characterMovementController;
        public CharacterMovementController CharacterMovementController
        {
            get { return this.characterMovementController; }
        }

        private AttributePool attributePool;
        public AttributePool AttributePOOL { get { return this.attributePool; } }

        private void Awake()
        {
            attributePool = GetComponentInChildren<AttributePool>();
            characterMovementController = CharacterMovementController.GetController(this.gameObject);
        }
    }
}