using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;
using GlowingObjects.Scripts;
using UnityEngine;
using ZonkaZombies.Prototype.Characters.Player;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private List<PlayerCharacterType> _validTypes = new List<PlayerCharacterType>();

        protected int Count;

        public virtual void OnAwake()
        {
            Count++;
        }

        public virtual void OnSleep()
        {
            Count--;

            Count = Mathf.Max(Count, 0);
        }

        public abstract void OnBegin(IInteractor interactor);
        public abstract void OnFinish();

        protected bool IsValidCharacter(Player player)
        {
            return player != null && _validTypes.Any(c => c == player.Type || c == PlayerCharacterType.All);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}