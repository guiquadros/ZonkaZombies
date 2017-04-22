using System.Collections.Generic;
using System.Linq;
using GlowingObjects.Scripts;
using UnityEngine;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Prototype.Characters.Player;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    //public class WeaponInteractable : MonoBehaviour, IInteractable { }
    //public class TriggerInteractable : MonoBehaviour, IInteractable { }
    //public class ActionInteractable : MonoBehaviour, IInteractable { }

    [RequireComponent(typeof(Collider))]
    public class CollectableInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private InteractableType _type;
        [SerializeField]
        private List<PlayerType> _validCharacterTypes;
        [SerializeField]
        private GlowableObject _glowableObject;
        public GameObject MainGameObject;

        private Collider _collider;
        private int _playersInteractingCount;

        public InteractableType Type { get { return _type; } }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void OnAwake()
        {
            _playersInteractingCount++;
            _glowableObject.Glow(true);
        }

        public void OnBegin(IInteractor interactor)
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

        public void OnFinish()
        {
            _collider.enabled = true;
        }

        public void OnSleep()
        {
            _playersInteractingCount--;

            _playersInteractingCount = Mathf.Max(_playersInteractingCount, 0);

            if (_playersInteractingCount == 0)
            {
                _glowableObject.Glow(false);
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}