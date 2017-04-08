using UnityEngine;
using UnityEngine.AI;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        protected NavMeshAgent agent;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update ()
        {
            if (target == null)
                return;

            agent.SetDestination(target.position);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
            {
                //The enemy was punched by the player
                //TODO Apply damage to the enemy
            }
        }
    }
}