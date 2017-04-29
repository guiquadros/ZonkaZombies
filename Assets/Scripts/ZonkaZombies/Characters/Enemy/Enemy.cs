using UnityEngine;
using UnityEngine.AI;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Characters.Enemy
{
    internal enum EnemyState
    {
        Pursuit,
        Patrol,
        Sleeping,
        LoseSight,
        Attack
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Character
    {
        private Player.Player _target;
        [SerializeField] private bool _useFieldOfView = false, _agentStopped = false;
        [SerializeField] private float _minPlayerDetectDistance = 5.0f, _fieldOfVisionTimeout = 5f;
        public float FieldOfViewAngle = 68.0f; // in degrees (I use 68, this gives the enemy a vision of 136 degrees) 

        protected NavMeshAgent Agent;

        private float _timeWithoutSeeingThePlayer;

        //TODO Move this to a ScriptableObject so It can be easily accessed by each enemy on scene
        [SerializeField] private AudioClip _damagedAudioClip;

        private EntityManager _entityManager;

        private EnemyState _currentState;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        protected override void Start()
        {
            base.Start();

            _entityManager = EntityManager.Instance;

            _currentState = EnemyState.Sleeping;
        }

#region ENEMY STATES

        protected virtual void OnPursuit()
        {
            if (_target == null)
                return;

            Agent.SetDestination(_target.transform.position);
            _timeWithoutSeeingThePlayer = 0f;

            if (_agentStopped)
                Agent.Resume();

            if (!CanSeePlayerCharacter())
            {
                _currentState = EnemyState.LoseSight;
                _timeWithoutSeeingThePlayer = 0;
            }
        }

        protected virtual void OnLoseSight()
        {
            _timeWithoutSeeingThePlayer += Time.deltaTime;

            //stops the pursuit after some time without see the player
            if (_timeWithoutSeeingThePlayer >= _fieldOfVisionTimeout)
            {
                _currentState = EnemyState.Sleeping;
                _timeWithoutSeeingThePlayer = 0f;
                Agent.Stop();
                _agentStopped = true;
            }
            else
            {
                if (CanSeePlayerCharacter())
                {
                    _currentState = EnemyState.Pursuit;
                }
            }
        }

        protected virtual void OnPatrol()
        {
            //TODO
        }

        protected virtual void OnSleeping()
        {
            if (CanSeePlayerCharacter())
            {
                _currentState = EnemyState.Pursuit;
            }
        }

        protected virtual void OnAttack()
        {
            //TODO
        }

#endregion

        protected virtual void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Pursuit:
                    OnPursuit();
                    break;
                case EnemyState.Patrol:
                    OnPatrol();
                    break;
                case EnemyState.Sleeping:
                    OnSleeping();
                    break;
                case EnemyState.LoseSight:
                    OnLoseSight();
                    break;
                case EnemyState.Attack:
                    OnAttack();
                    break;
            }
        }

        protected virtual void FixedUpdate()
        {
            _target = _entityManager.GetNearestPlayer(this);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                Player.Player abstractPlayerCharacter = other.gameObject.GetComponent<Player.Player>();

                abstractPlayerCharacter.Damage(HitPoints);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            //The enemy was punched by the player
            if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
            {
                var playerCharacter = other.gameObject.GetComponentInParent<Player.Player>();
                Damage(playerCharacter.HitPoints);
            }
        }

        protected override void OnTakeDamage(int damage)
        {
            AudioManager.Instance.PlayEffect(_damagedAudioClip);
        }

        private bool CanSeePlayerCharacter()
        {
            if (!_useFieldOfView) return true;

            if (_target == null)
            {
                return false;
            }

            RaycastHit hit;
            Vector3 rayDirection = _target.transform.position - transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _target.transform.position);

            //raycast to the player direction
            bool raycastObj = Physics.Raycast(transform.position, rayDirection, out hit);
            
            if (!raycastObj)
            {
                return false;
            }

            Debug.DrawLine(transform.position, hit.point, Color.yellow);

            //verify if the object hit is a player character
            bool playerHit = hit.transform.CompareTag(TagConstants.PLAYER);

            //verify if the distance to the player matches the min distance
            bool matchedMinDistance = distanceToPlayer <= _minPlayerDetectDistance;

            float angle = Vector3.Angle(rayDirection, transform.forward);

            //verify the player is in the angle range of the field of vision of the enemy
            bool matchedFieldOfVisionAngle = angle < FieldOfViewAngle;

            return playerHit && (matchedMinDistance || matchedFieldOfVisionAngle);
        }
    }
}