using UnityEngine;

namespace ZonkaZombies.Prototype.Scenery
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerCharacterTransform;

        private void Update()
        {
            Vector3 currentCameraPosition = this.transform.position;
            this.transform.position = new Vector3(_playerCharacterTransform.position.x, currentCameraPosition.y, currentCameraPosition.z);
        }
    }
}
