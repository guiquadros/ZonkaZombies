using UnityEngine;
using UnityEngine.AI;

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
            if (other.gameObject.layer == 8)
            {
                Destroy(other.gameObject);
            }
        }
    }
}