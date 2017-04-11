using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Prototype.Characters.Enemy;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class BulletBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 15f;

        [SerializeField]
        private int _bulletHitPoints = 1;

        private void Awake()
        {
            Destroy(this.gameObject, 5.0f);
        }

        private void Update()
        {
            Vector3 deltaPos = Time.deltaTime * _bulletSpeed * transform.forward;
            transform.position += deltaPos;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.ENEMY_LAYER)
            {
                EnemyBehavior characterBehavior = other.gameObject.GetComponent<EnemyBehavior>();
                characterBehavior.Damage(_bulletHitPoints, () => Destroy(other.gameObject));
            }
        }
    }
}
