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
                PoliceOfficerBehavior policeOfficer = GameObject.FindObjectOfType<PoliceOfficerBehavior>();

                EnemyBehavior characterBehavior = other.gameObject.GetComponent<EnemyBehavior>();
                characterBehavior.Damage(policeOfficer.ShotHitPoints, () => Destroy(other.gameObject));
            }
        }
    }
}
