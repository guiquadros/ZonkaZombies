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
        private Transform _target;

        [SerializeField]
        private Transform _playerCharacterTransform;

        [SerializeField]
        private bool _useFieldOfView = false;

        protected NavMeshAgent Agent;

        [SerializeField]
        private float _minPlayerDetectDistance = 5.0f, _fieldOfViewRange = 68.0f;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            if (_target == null)
                return;

            if (CanSeePlayerCharacter())
            {
                Agent.SetDestination(_target.position);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                PlayerCharacterBehavior playerCharacter = other.gameObject.GetComponent<PlayerCharacterBehavior>();
#if UNITY_EDITOR
                if (playerCharacter.CanReceiveDamage)
#endif
                    playerCharacter.Damage(HitPoints, () => SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //The enemy was punched by the player
            if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
            {
                var playerCharacter = other.gameObject.GetComponentInParent<PlayerCharacterBehavior>();
                this.Damage(playerCharacter.HitPoints, () => Destroy(this.gameObject));
            }
        }

        private bool CanSeePlayerCharacter()
        {
            if (!_useFieldOfView) return true;

            RaycastHit hit;
            var rayDirection = _playerCharacterTransform.position - transform.position;
            var distanceToPlayer = Vector3.Distance(transform.position, _playerCharacterTransform.position);

            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                // If the player is very close behind the player and in view the enemy will detect the player
                if ((hit.transform.CompareTag("Player")) && (distanceToPlayer <= _minPlayerDetectDistance))
                {
                    return true;
                }
            }

            if ((Vector3.Angle(rayDirection, transform.forward)) < _fieldOfViewRange)
            {
                // Detect if player is within the field of view
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {

                    if (hit.transform.CompareTag(TagConstants.PLAYER))
                    {
                        Debug.Log("Can see player");
                        return true;
                    }
                    else
                    {
                        Debug.Log("Can not see player");
                        return false;
                    }
                }
            }

            return false;
        }
    }
}