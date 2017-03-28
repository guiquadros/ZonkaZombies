using UnityEngine;

namespace ZonkaZombies.Prototype.PlayerCharacter
{
    public class BulletBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 15f;

        private void Update()
        {
            Vector3 deltaPos = Time.deltaTime * _bulletSpeed * transform.forward *-1;
            transform.position += deltaPos;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == 11)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
