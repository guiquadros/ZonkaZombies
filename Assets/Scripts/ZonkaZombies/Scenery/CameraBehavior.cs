using UnityEngine;

namespace ZonkaZombies.Scenery
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerCharacterTransform;

        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position - _playerCharacterTransform.position;
        }

        private void LateUpdate()
        {
            transform.position = _offset + _playerCharacterTransform.position;
        }
    }
}
