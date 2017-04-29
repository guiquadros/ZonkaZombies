using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public class DoorInteractable : InteractableGlowable
    {
        private bool _isOpened;

        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private Collider _collider;

        private Color _transparentGreen;

        private void Start()
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

        public override void OnFinish()
        {
            // Not necessary...
        }

        private void UpdateDoorState()
        {
            if (_renderer == null || _collider == null)
            {
                return;
            }

            _renderer.material.color = _isOpened ? _transparentGreen : Color.red;
            _collider.enabled = !_isOpened;
        }
    }
}