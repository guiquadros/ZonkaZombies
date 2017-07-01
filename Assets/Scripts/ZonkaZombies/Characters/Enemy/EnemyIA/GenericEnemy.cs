using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animations.Character;
using UnityEngine;
using UnityEngine.AI;
using ZonkaZombies.Characters.Enemy.Data;
using ZonkaZombies.Characters.Enemy.EnemyIA.General;
using ZonkaZombies.Characters.Player;
using ZonkaZombies.Managers;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.Util;
using Random = UnityEngine.Random;

// ReSharper disable All

namespace ZonkaZombies.Characters.Enemy.EnemyIA
{
    /// <summary>
    /// Represents an generic enemy that implements an Finite State Machine to handle its EEnemyStates.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class GenericEnemy : Character, IResetable
    {
        #region SerializeFields and Public Attributes
        [SerializeField]
        private Collider _hitBoxCollider;

        [SerializeField]
        private bool _useFieldOfView = false;

        [SerializeField]
        private float _minPlayerDetectDistance = 5.0f;

        //TODO Move this to a ScriptableObject so It can be easily accessed by each enemy on scene
        [SerializeField]
        private AudioClip _damagedAudioClip;

        [SerializeField]
        internal Animator Animator;

        [SerializeField]
        internal NavMeshAgent Agent;

        [SerializeField]
        private EnemyDetails _enemyDetails;

        [SerializeField]
        private Player.Behaviors.Player _characterTarget;

        [SerializeField]
        private EEnemyBehavior _defaultEEnemyBehavior = EEnemyBehavior.Sleep;

        public float FieldOfViewAngle = 68.0f; // in degrees (I use 68, this gives the enemy a vision of 136 degrees) 
        #endregion

        #region Private Attributes
        private List<BaseEnemyBehavior> _availableBehaviors;
        private List<BaseEnemyBehavior> _activeBehaviors;
        private EEnemyBehavior _activeBehavior;

        #endregion

        #region Properties

        public EnemyDetails EnemyDetails
        {
            get { return _enemyDetails ?? new EnemyDetails(); }
        }
        
        public bool HasTarget { get { return _characterTarget != null && !_characterTarget.Equals(null); } }

        public bool UseFieldOfView
        {
            get { return _useFieldOfView; }
            set { _useFieldOfView = value; }
        }

        internal Player.Behaviors.Player CharacterTarget { get { return _characterTarget; } }

        internal bool FreezeCurrentTarget;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _availableBehaviors = GetComponents<BaseEnemyBehavior>().ToList();

            // Make sure all states start disabled
            _availableBehaviors.ForEach(b => b.enabled = false);

            var generalStateMachineBehaviours = Animator.GetBehaviours<GeneralStateMachineBehaviour>();

            foreach (var generalStateMachineBehaviour in generalStateMachineBehaviours)
            {
                generalStateMachineBehaviour.StateExit += GeneralStateMachineBehaviour_OnStateExit;
            }
        }

        private void GeneralStateMachineBehaviour_OnStateExit(EEnemyBehavior enemyBehaviorStateExit)
        {
            switch (enemyBehaviorStateExit)
            {
                case EEnemyBehavior.Die:
                    Destroy(gameObject);
                    break;
                case EEnemyBehavior.Attack:
                    _hitBoxCollider.enabled = false;
                    break;
            }
        }

        private void Start()
        {
            TryChangeBehavior(_defaultEEnemyBehavior);

            MessageRouter.SendMessage(new OnEnemyHasBornMessage(this));
        }

        public bool HasBehavior(EEnemyBehavior searchBehavior)
        {
            List<BaseEnemyBehavior> behaviors;
            return TryFindBehavior(searchBehavior, out behaviors);
        }

        private bool TryFindBehavior(EEnemyBehavior searchBehavior, out List<BaseEnemyBehavior> result)
        {
            result = _availableBehaviors.FindAll(b => b.GetValidStates().Contains(searchBehavior));
            return result != null && result.Count > 0;
        }

        internal void TryChangeBehavior(EEnemyBehavior newBehavior)
        {
            //Debug.LogFormat("Enemy {0} is trying to change from state {1} to {2}.", gameObject.name, _activeBehavior, newBehavior);

            // Disable active behaviors, if exists
            if (_activeBehaviors != null)
            {
                foreach (var activeState in _activeBehaviors)
                {
                    activeState.enabled = false;
                }
            }

            if (TryFindBehavior(newBehavior, out _activeBehaviors))
            {
                // Initialize and enable new behaviors
                foreach (BaseEnemyBehavior enemyBehavior in _activeBehaviors)
                {
                    if (!enemyBehavior.IsInitialized)
                    {
                        // Lazy initialization
                        enemyBehavior.Initialize(this);
                    }

                    enemyBehavior.enabled = true;
                }
            }
            else
            {
                // Fallback to a default state
                TryChangeBehavior(_defaultEEnemyBehavior);
            }
        }

        public void FollowPlayerPosition()
        {
            Agent.SetDestination(_characterTarget.transform.position);
        }

        public bool CanSeePlayerCharacter()
        {
            if (!FreezeCurrentTarget || !HasTarget)
            {
                FindNearestPlayer();
            }

            if (!_useFieldOfView)
            {
                return true;
            }

            if (_characterTarget == null)
            {
                return false;
            }

            RaycastHit hit;
            Vector3 rayDirection = _characterTarget.transform.position - transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _characterTarget.transform.position);

            //this is necessary to avoid raycast the floor always
            rayDirection.y = 1f;

            //TODO This is not working!
            LayerMask sceneryAndPlayerMask = new LayerMask
            {
                value = LayerConstants.PLAYER_CHARACTER_LAYER | LayerConstants.SCENERY_LAYER
            };
            //raycast to the player direction
            bool raycastObj = Physics.Raycast(transform.position, rayDirection, out hit, sceneryAndPlayerMask);
            
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

        public void FindNearestPlayer()
        {
            _characterTarget = EntityManager.Instance.GetNearestPlayer(transform);
        }

        protected override void OnDamaged()
        {
            TryChangeBehavior(EEnemyBehavior.TakeDamage);
        }

        #region Enemy Behavior Events
        public event Action<Collider> EnemyTriggerEnter;
        public event Action<Collider> EnemyTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (CanReceiveDamage()) // Optimizes performance
            {
                if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
                {
                    // The enemy must always takes damage from the player, so It is not a behavior exclusive logic.
                    // This logic will be called only if the player is punching OR attacking with a white gun.
                    PlayerHelper helper = other.gameObject.GetComponent<PlayerHelper>();

                    if (helper == null)
                    {
                        Debug.LogError("The player has not a PlayerHelper!");
                        return;
                    }

                    Damage(helper.Player.Hit.Current);
                }
            }

            if (EnemyTriggerEnter != null)
            {
                EnemyTriggerEnter(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (EnemyTriggerExit != null)
            {
                EnemyTriggerExit(other);
            }
        }
        #endregion

        internal IEnumerator AutoGroom(float minVolume = 1f, float maxVolume = 1f)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(3, 15));

                AudioManager.Instance.Play(EnemyDetails.AnyMurmur, Random.Range(minVolume, maxVolume));
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
    }
}