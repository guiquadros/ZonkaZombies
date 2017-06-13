using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Characters.Player;
using ZonkaZombies.Characters.Player.Behaviors;

namespace ZonkaZombies.Scenery.Interaction
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private List<PlayerCharacterType> _validTypes = new List<PlayerCharacterType>();

        protected int Count;

        public delegate void OnGet(InteractableBase interactable, Player player, object[] args);
        public event OnGet OnInteract;

        public virtual void OnAwake()
        {
            Count++;
        }

        public virtual void OnSleep()
        {
            Count--;

            Count = Mathf.Max(Count, 0);
        }

        protected void ResetCounter()
        {
            Count = 0;
        }

        public abstract void OnBegin(IInteractor interactor);
        public abstract void OnFinish(IInteractor interactor);

        protected bool IsValidCharacter(Player player)
        {
            return player != null && _validTypes.Any(c => c == player.Type || c == PlayerCharacterType.All);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        protected void DispatchOnInteractEvent(Player player, params object[] args)
        {
            if (OnInteract != null)
            {
                OnInteract(this, player, args);
            }
        }
    }
}