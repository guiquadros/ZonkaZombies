using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using ZonkaZombies.Prototype.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Character
    {
        [SerializeField] private Transform _target;

        [SerializeField] private Transform _playerCharacterTransform;

        [SerializeField] private bool _useFieldOfView = false, _agentStopped = false;

        protected NavMeshAgent Agent;

        [SerializeField] private float _minPlayerDetectDistance = 5.0f, _fieldOfVisionTimeout = 5f;

        public float FieldOfViewAngle = 68.0f; // in degrees (I use 68, this gives the enemy a vision of 136 degrees) 

        private float _timeWithoutSeeingThePlayer;

        //TODO Move this to a ScriptableObject so It can be easily accessed by each enemy on scene
        [SerializeField] private AudioClip _damagedAudioClip;

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
                //Debug.Log("Can see the player");
                Agent.SetDestination(_target.position);
                _timeWithoutSeeingThePlayer = 0f;

                if (_agentStopped)
                {
                    Agent.Resume();
                }
            }
            else
            {
                //Debug.Log("Can NOT see the player");
                _timeWithoutSeeingThePlayer += Time.deltaTime;
            }

            //stops the pursuit after some time without see the player
            if (_timeWithoutSeeingThePlayer >= _fieldOfVisionTimeout)
            {
                _timeWithoutSeeingThePlayer = 0f;
                Agent.Stop();

                _agentStopped = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                Player.Player abstractPlayerCharacter = other.gameObject.GetComponent<Player.Player>();

                abstractPlayerCharacter.Damage(HitPoints, () => SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //The enemy was punched by the player
            if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
            {
                var playerCharacter = other.gameObject.GetComponentInParent<Player.Player>();
                this.Damage(playerCharacter.HitPoints, () => Destroy(this.gameObject));
            }
        }

        protected override void OnTakeDamage(int damage)
        {
            AudioManager.Instance.PlayEffect(_damagedAudioClip);
        }

        private bool CanSeePlayerCharacter()
        {
            if (!_useFieldOfView) return true;

            RaycastHit hit;
            Vector3 rayDirection = _playerCharacterTransform.position - transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _playerCharacterTransform.position);

            //raycast to the player direction
            bool raycastObj = Physics.Raycast(transform.position, rayDirection, out hit);

            //verify if the object hit is a player character
            bool playerHit = hit.transform.CompareTag(TagConstants.PLAYER);

            //verify if the distance to the player matches the min distance
            bool matchedMinDistance = distanceToPlayer <= _minPlayerDetectDistance;

            float angle = Vector3.Angle(rayDirection, transform.forward);

            //verify the player is in the angle range of the field of vision of the enemy
            bool matchedFieldOfVisionAngle = angle < FieldOfViewAngle;

            return raycastObj && playerHit && (matchedMinDistance || matchedFieldOfVisionAngle);
        }
    }
}