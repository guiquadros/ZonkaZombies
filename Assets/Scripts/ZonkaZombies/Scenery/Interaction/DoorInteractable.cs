using UnityEngine;
using UnityEngine.AI;

namespace ZonkaZombies.Scenery.Interaction
{
    public class DoorInteractable : InteractableGlowable
    {
        private bool _isOpened;

        [SerializeField] protected Renderer _renderer;

        [SerializeField]
        protected Collider _collider;

        [SerializeField] protected NavMeshObstacle _navMeshObstacle;

        protected Color _transparentGreen;

        protected virtual void Start()
        {
            _transparentGreen = Color.green;
            _transparentGreen.a = .5f;

            _isOpened = false;
            UpdateDoorState();
        }

        public override void OnBegin(IInteractor interactor)
        {
            _isOpened = !_isOpened;
            UpdateDoorState();
        }

        public override void OnFinish(IInteractor interactor)
        {
            // Not necessary...
        }

        protected virtual void UpdateDoorState()
        {
            if (_renderer == null || _collider == null)
            {
                return;
            }

            _renderer.material.color = _isOpened ? _transparentGreen : Color.red;
            _navMeshObstacle.enabled = !_isOpened;
            _collider.enabled = !_isOpened;
        }
    }
}