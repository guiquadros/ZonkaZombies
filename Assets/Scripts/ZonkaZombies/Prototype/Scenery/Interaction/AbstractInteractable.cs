using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class AbstractInteractable : MonoBehaviour
    {
        [SerializeField]
        private InteractableType _type;
        [SerializeField]
        private List<PlayerCharacterType> _validCharacterTypes;

        public GameObject MainGameObject;

        private Collider _collider;

        public InteractableType Type { get { return _type; } }
        public PlayerCharacterBehavior PlayerBehaviourInteracting { get; private set; }
        public bool IsSomeoneInteractingWithIt { get { return PlayerBehaviourInteracting != null; } }

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public virtual bool IsValidForInteraction(PlayerCharacterType characterType)
        {
            return _validCharacterTypes.Any(t => t == characterType);
        }

        public void StartInteraction(PlayerCharacterBehavior characterBehaviour)
        {
            PlayerBehaviourInteracting = characterBehaviour;
            _collider.enabled = false;
        }

        public void FinishInteraction()
        {
            PlayerBehaviourInteracting = null;
            _collider.enabled = true;
        }
    }
}