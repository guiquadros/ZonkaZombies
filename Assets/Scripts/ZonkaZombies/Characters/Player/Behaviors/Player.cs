using System;
using UnityEngine;
using ZonkaZombies.Characters.Enemy;
using ZonkaZombies.Characters.Player.Util;
using ZonkaZombies.Characters.Player.Weapon;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;
using ZonkaZombies.Characters.Player.Interaction;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.UI.Data;
using ZonkaZombies.UI.Dialogues;

namespace ZonkaZombies.Characters.Player.Behaviors
{
    [RequireComponent(typeof(InteractionHandler), typeof(MonoBehaviour), typeof(Rigidbody))]
    public abstract class Player : Character
    {
        public bool IsFirstPlayer { get { return InputType != InputType.Controller2; } }

        [SerializeField]
        protected Animator Animator;

        [SerializeField, Header("Movement Settings")]
        private float _speed = 6f;
        [SerializeField, Range(0, 1440)]
        private float _angularSpeed = 720.0f;

        [Header("Input Settings")]
        public InputType InputType = InputType.Controller1;
        private InputType _oldInputType;
        [Range(0, 2), SerializeField]
        private float _punchCooldown = .25f;

        [Header("Debug Attributes")]
        [SerializeField]
        private bool _doNotReceiveDamage;
        [SerializeField]
        private bool _frezeePlayer;
        private bool _canPunch = true;
        private bool _isRotating;
        public bool CanMove { get; internal set; }
        public bool CanPunch
        {
            get
            {
                return _canPunch && !_weaponSelector.IsEquippingFireGun;
            }
        }
        public bool CanRotate { get; internal set; }
        public bool CanInteract { get; internal set; }
        public bool IsMoving { get; internal set; }

        public PlayerCharacterType Type { get; internal set; }

        private InputReader _inputReader;
        private InteractionHandler _interactionHandler;

        private Rigidbody _characterRigidbody;
        private Vector3 _movement;
        private Quaternion _endRotation = Quaternion.identity;
        private IWeaponSelector _weaponSelector;

        public delegate void WeaponChanged(WeaponDetails weaponDetails, Player player);
        public WeaponChanged OnWeaponChanged;

        /// <summary>
        /// TODO Find a better way of setting substates for the characters that can be reutilized by the enemies.
        /// </summary>
        /// <param name="states"></param>
        public void SetState(params CharacterStateMessage[] states)
        {
            foreach (CharacterStateMessage state in states)
            {
                switch (state.Type)
                {
                    case CharacterMechanicType.Movement:
                        CanMove = state.Value;
                        break;
                    case CharacterMechanicType.Punch:
                        _canPunch = state.Value;
                        break;
                    case CharacterMechanicType.Rotation:
                        CanRotate = state.Value;
                        break;
                    case CharacterMechanicType.Interaction:
                        CanInteract = state.Value;
                        break;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _frezeePlayer = !Animator;

            _inputReader = PlayerInput.GetInputReader(InputType);
            _oldInputType = InputType;

            _interactionHandler = GetComponent<InteractionHandler>();

            if (_interactionHandler == null)
            {
                Debug.LogError("InteractionBehaviour cannot be null!");
                return;
            }

            _interactionHandler.SetUp(this);

            _characterRigidbody = GetComponent<Rigidbody>();

            // Find and holds reference to the WeaponSelector
            WeaponSelector weaponSelector = GetComponentInChildren<WeaponSelector>();
            weaponSelector.Initialize(this, _inputReader);
            _weaponSelector = weaponSelector;

            ((WeaponSelector)_weaponSelector).Initialize(this, _inputReader);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState(CharacterStateMessage.EnableMovement, 
                     CharacterStateMessage.EnableRotation, 
                     CharacterStateMessage.EnablePunch, 
                     CharacterStateMessage.EnableInteraction);

            MessageRouter.SendMessage(new OnPlayerHasBornMessage(this));
        }

        private void OnEnable()
        {
            DialogueManager.Instance.DialogueStarted += OnDialogueStarted;
            DialogueManager.Instance.DialogueFinished += OnDialogueFinished;
            SceneController.Instance.OnSceneLoading += OnOnSceneLoading;
            SceneController.Instance.AfterSceneLoad += OnAfterSceneLoad;
        }
        
        private void OnDisable()
        {
            DialogueManager.Instance.DialogueStarted -= OnDialogueStarted;
            DialogueManager.Instance.DialogueFinished -= OnDialogueFinished;
            SceneController.Instance.OnSceneLoading -= OnOnSceneLoading;
            SceneController.Instance.AfterSceneLoad -= OnAfterSceneLoad;
        }

        private void OnOnSceneLoading(GameSceneType gameSceneType)
        {
            FreezePlayer();
        }

        private void OnAfterSceneLoad()
        {
            UnfreezePlayer();
        }
        
        private void OnDialogueFinished(Dialogue dialogue, bool freezePlayers)
        {
            if (freezePlayers)
            {
                UnfreezePlayer();
            }
        }
        
        private void OnDialogueStarted(Dialogue dialogue, Transform interactableTransform, bool freezePlayers)
        {
            if (freezePlayers)
            {
                FreezePlayer();

                if (interactableTransform != null)
                {
                    transform.LookAt(interactableTransform);
                }
            }
        }

        private void FreezePlayer()
        {
            _characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _frezeePlayer = true;

            Animator.SetFloat(PlayerAnimatorParameters.MOVEMENT_DIRECTION, -1);
            Animator.SetTrigger(PlayerAnimatorParameters.FORCE_IDLE);
        }

        private void UnfreezePlayer()
        {
            _characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ |
                                              RigidbodyConstraints.FreezeRotationX;
            _frezeePlayer = false;
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR || CHEAT_ENABLED
            if (
                (UnityEngine.Input.GetKey(KeyCode.RightControl) || UnityEngine.Input.GetKey(KeyCode.LeftControl))
                && (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
                && UnityEngine.Input.GetKey(KeyCode.F2)
            )
            {
                this._doNotReceiveDamage = true;
            }

            if (
                (UnityEngine.Input.GetKey(KeyCode.RightControl) || UnityEngine.Input.GetKey(KeyCode.LeftControl))
                && (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
                && UnityEngine.Input.GetKey(KeyCode.F3)
            )
            {
                this._doNotReceiveDamage = false;
            }
#endif

            if (_frezeePlayer) return;

            HandleMovement();
            HandleRotation();
            HandleDiscardWeapon();
            HandleInteraction();
            bool weaponbuttonPressed = HandleWeapon(); 
            bool punchButtonPressed = HandlePunch();

            //idle animation
            Animator.SetBool(PlayerAnimatorParameters.IDLE, !IsMoving && !_isRotating && !punchButtonPressed && !weaponbuttonPressed);

            // Debug methods
            DebugChangeInputInGame();
        }

        protected override bool CanReceiveDamage()
        {
            return !_doNotReceiveDamage;
        }

        protected bool HandlePunch()
        {
            if (!CanPunch)
            {
                return false;
            }

            if (_inputReader.AnyBumperDown())
            {
                Animator.SetTrigger(PlayerAnimatorParameters.PUNCH);
                return true;
            }

            return false;
        }

        protected virtual void HandleMovement()
        {
            //translation
            _movement = Camera.main.transform.right   * _inputReader.LeftAnalogStickHorizontal() +
                        Camera.main.transform.forward * _inputReader.LeftAnalogStickVertical();
            _movement.y = 0; // Forces the Y axis to be 0

            IsMoving = _movement != Vector3.zero && CanMove;

            float walking = -1;

            if (IsMoving)
            {
                walking = (-Vector2.Dot(new Vector2(_movement.x, _movement.z), new Vector2(transform.forward.x, transform.forward.z)) + 1) * 0.5f;
                walking = Mathf.Clamp01(walking);
            }

            Animator.SetFloat(PlayerAnimatorParameters.MOVEMENT_DIRECTION, walking);

            if (!IsMoving)
            {
                // Don't need to do the raycast and move the rigidbody if the player didn't applies any input
                return;
            }

            Ray ray = new Ray(transform.position, -transform.up * .5f);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, LayerConstants.FLOOR_LAYER))
            {
                _movement = Vector3.ProjectOnPlane(_movement, hitInfo.normal.normalized);
            }

            _movement = _movement * _speed * Time.deltaTime;
            _characterRigidbody.MovePosition(transform.position + _movement);
        }

        protected virtual void HandleRotation()
        {
            if (!CanRotate)
            {
                return;
            }
            
            //rotation
            //TODO Review this fucked code, later! We cannot negate the result of the Stick values!
            Vector3 rotationDirection = Camera.main.transform.right   * -_inputReader.RightAnalogStickHorizontal() +
                                        Camera.main.transform.forward * -_inputReader.RightAnalogStickVertical();

            rotationDirection.y = 0;

            _isRotating = false;

            float rotationX = Mathf.Abs(rotationDirection.x);
            float rotationZ = Mathf.Abs(rotationDirection.z);

            if (rotationX >= .2f || rotationZ >= .2f)
            {
                _isRotating = true;
                _endRotation = Quaternion.LookRotation(rotationDirection);
                if (!IsMoving)
                {
                    Animator.SetFloat(PlayerAnimatorParameters.MOVEMENT_DIRECTION, 0.5f);
                }
            }

            Vector3 tempRotation = transform.rotation.eulerAngles;
            tempRotation.y = Mathf.MoveTowardsAngle(tempRotation.y, _endRotation.eulerAngles.y, _angularSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(tempRotation);
        }

        protected virtual void HandleInteraction()
        {
            if (!CanInteract)
            {
                return;
            }
            
            if (_inputReader.ADown())
            {
                _interactionHandler.Execute();
            }
        }

        protected virtual void HandleDiscardWeapon()
        {
            if (_inputReader.BDown() && _interactionHandler.CanDiscardWeapon)
            {
                if (_weaponSelector.Discard())
                {
                    DispatchOnWeaponChangedEvent();
                }
            }
        }

        protected virtual bool HandleWeapon()
        {
            if (_inputReader.AnyTrigger())
            {
                _weaponSelector.TryToUse();
                return true;
            }

            return false;
        }

        public void SelectWeapon(WeaponModel model)
        {
            if (_weaponSelector.Select(model))
            {
                DispatchOnWeaponChangedEvent();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // This test being done here improves the performance of damage logic. It is not necessary to do get the EnemyHelper component all the time.
            if (CanReceiveDamage())
            {
                if (other.CompareTag(TagConstants.ENEMY_DAMAGER))
                {
                    EnemyHelper enemyHelper = other.gameObject.GetComponent<EnemyHelper>();

                    if (enemyHelper == null)
                    {
                        Debug.LogErrorFormat("The {0} enemy's damager has hit the player but It doesn't has a EnemyHelper Component!", other.gameObject.name);
                        return;
                    }

                    Vector3 direction = enemyHelper.Enemy.transform.position - transform.position;
                    float angle = Vector3.Angle(transform.forward, direction);

                    Animator.SetTrigger(Mathf.Abs(angle) > 30
                        ? PlayerAnimatorParameters.DAMAGE_BACK
                        : PlayerAnimatorParameters.DAMAGE_FRONT);
                    
                    // Takes damage from the player
                    Damage(enemyHelper.Enemy.Hit.Current);
                }
            }
        }

        private void OnDestroy()
        {
            EntityManager.Instance.Players.Remove(this);
        }

        #region Debug Methods

        private void DebugChangeInputInGame()
        {
            if (_oldInputType != InputType)
            {
                Debug.LogFormat("<color=orange>DEBUG: Input changed from {0} to {1}.</color>", _oldInputType, InputType);
                _inputReader = PlayerInput.GetInputReader(InputType);
                ((WeaponSelector)_weaponSelector).Initialize(this, _inputReader);
            }
            _oldInputType = InputType;
        }

        #endregion

        public WeaponDetails GetWeapon()
        {
            return _weaponSelector.GetWeapon();
        }

        private void DispatchOnWeaponChangedEvent()
        {
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(GetWeapon(), this);
            }
        }

    }
}