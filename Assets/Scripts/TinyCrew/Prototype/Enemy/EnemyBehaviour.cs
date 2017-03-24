using UnityEngine;
using UnityEngine.AI;

namespace TinyCrew.Prototype.Enemy
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
    }
}