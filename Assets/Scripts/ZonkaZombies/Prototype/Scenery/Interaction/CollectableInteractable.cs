using UnityEngine;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    //public class WeaponInteractable : MonoBehaviour, IInteractable { }
    //public class TriggerInteractable : MonoBehaviour, IInteractable { }
    //public class ActionInteractable : MonoBehaviour, IInteractable { }

    [RequireComponent(typeof(Collider))]
    public class CollectableInteractable : InteractableGlowable
    {
        [SerializeField]
        private InteractableType _type;

        private Collider _collider;

        public InteractableType Type { get { return _type; } }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public override void OnBegin(IInteractor interactor)
        {
            Player player = interactor.GetCharacter() as Player;

            if (IsValidCharacter(player))
            {
                _collider.enabled = false;

                //TODO Add the item to the player's inventory

                Debug.LogFormat("'{0}' added to the character's inventory!", gameObject.name);

                Destroy(gameObject);
            }
        }

        public override void OnFinish() { }
    }
}