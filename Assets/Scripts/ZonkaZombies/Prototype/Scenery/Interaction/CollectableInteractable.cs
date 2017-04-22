using System.Collections.Generic;
using System.Linq;
using GlowingObjects.Scripts;
using UnityEngine;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Prototype.Characters.Player;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    public class DestructableInteractable : Interactable
    {
        public override void OnAwake()
        {
            
        }

        public override void OnBegin(IInteractor interactor)
        {
            
        }

        public override void OnFinish()
        {
            
        }

        public override void OnSleep()
        {
            
        }
    }

    //public class WeaponInteractable : MonoBehaviour, IInteractable { }
    //public class TriggerInteractable : MonoBehaviour, IInteractable { }
    //public class ActionInteractable : MonoBehaviour, IInteractable { }

    [RequireComponent(typeof(Collider))]
    public class CollectableInteractable : Interactable
    {
        [SerializeField]
        private InteractableType _type;
        [SerializeField]
        private List<PlayerType> _validCharacterTypes;

        private Collider _collider;

        public InteractableType Type { get { return _type; } }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public override void OnBegin(IInteractor interactor)
        {
            Player player = interactor.GetCharacter() as Player;

            bool isValidCharacter = player != null && _validCharacterTypes.Any(t => t == player.Type);

            if (isValidCharacter)
            {
                //TODO Check to know if the character has the correct object on its inventory, for example...

                _collider.enabled = false;
                
                //TODO
            }
        }

        public override void OnFinish()
        {
            _collider.enabled = true;
        }
    }
}