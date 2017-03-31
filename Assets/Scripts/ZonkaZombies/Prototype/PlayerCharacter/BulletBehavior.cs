using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.PlayerCharacter
{
    public class BulletBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 15f;

        private void Update()
        {
            Vector3 deltaPos = Time.deltaTime * _bulletSpeed * transform.forward;
            transform.position += deltaPos;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.ENEMY_LAYER)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
