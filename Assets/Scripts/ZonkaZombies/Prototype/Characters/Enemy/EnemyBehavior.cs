using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBehavior : CharacterBehavior
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
                PlayerCharacterBehavior characterBehavior = other.gameObject.GetComponent<PlayerCharacterBehavior>();
                characterBehavior.Damage(HitPoints, () => SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME));
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