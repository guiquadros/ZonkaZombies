using UnityEngine;
using ZonkaZombies.Characters.Player.Behaviors;

namespace ZonkaZombies.Scenery.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class CollectableInteractable : InteractableGlowable, ICollectable
    {
        [SerializeField]
        private InteractableType _type;

        private Collider _collider;

        public InteractableType Type { get { return _type; } }

        private bool _isBeingInteracted;

        protected Player PlayerInteracting;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public override void OnBegin(IInteractor interactor)
        {
            if (_isBeingInteracted)
            {
                return;
            }

            _isBeingInteracted = true;

            Player player = interactor.GetCharacter() as Player;

            if (IsValidCharacter(player))
            {
                PlayerInteracting = player;

                _collider.enabled = false;

                //TODO Add the item to the player's inventory

                Debug.LogFormat("'{0}' added to the character's inventory!", gameObject.name);


                interactor.ForceFinish();

                Collect();
            }

            _isBeingInteracted = false;
        }

        public override void OnFinish(IInteractor interactor) { }

        public virtual void Collect()
        {
            DispatchOnInteractEvent(PlayerInteracting);
            Destroy(gameObject);
        }

        protected void ResetInteractable()
        {
            ResetGlow();
            ResetCounter();
            _collider.enabled = true;
        }
    }
}