using UnityEngine;
using ZonkaZombies.Input;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;
using ZonkaZombies.Prototype.Scenery.Interaction;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.Player
{
    [RequireComponent(typeof(InteractionHandler), typeof(MonoBehaviour), typeof(Rigidbody))]
    public class Player : Character
    {
        [SerializeField]
        protected Animator Animator;

        [SerializeField, Header("Movement Settings")]
        private float _speed = 6f;
        [SerializeField, Range(0, 1440)]
        private float _angularSpeed = 720.0f;

        [SerializeField, Header("Input Settings")]
        private InputType _inputType = InputType.Controller1;

        [Header("Debug Attributes")]
        public bool CanReceiveDamage = true;

        [SerializeField]
        private bool _frezeePlayer = false;

        public bool CanMove { get; internal set; }
        public bool CanPunch { get; internal set; }
        public bool CanRotate { get; internal set; }
        public bool CanInteract { get; internal set; }
        public bool IsMoving { get; internal set; }
        public PlayerCharacterType Type { get; internal set; }

        public InputReader InputReader;
        private ICommand _interactionHandler;

        private Rigidbody _characterRigidbody;
        private Vector3 _movement;
        private Quaternion _endRotation = Quaternion.identity;

        /// <summary>
        /// TODO Find a better way of setting substates for the characters that can be reutilized by the enemies.
        /// </summary>
        /// <param name="states"></param>
        public void SetState(params CharacterState[] states)
        {
            foreach (CharacterState state in states)
            {
                switch (state)
                {
                    case CharacterState.CantMove:
                        CanMove = false;
                        break;
                    case CharacterState.CanMove:
                        CanMove = true;
                        break;
                    case CharacterState.CantPunch:
                        CanPunch = false;
                        break;
                    case CharacterState.CanPunch:
                        CanPunch = true;
                        break;
                    case CharacterState.CantRotate:
                        CanRotate = false;
                        break;
                    case CharacterState.CanRotate:
                        CanRotate = true;
                        break;
                    case CharacterState.CantInteract:
                        CanInteract = false;
                        break;
                    case CharacterState.CanInteract:
                        CanInteract = true;
                        break;
                }
            }
        }

        protected virtual void Awake()
        {
            _frezeePlayer = !Animator;
            _interactionHandler = GetComponent<ICommand>();

            if (_interactionHandler == null)
            {
                Debug.LogError("InteractionBehaviour cannot be null!");
                Application.Quit();
            }

            (_interactionHandler as IInteractor).SetUp(this);

            _characterRigidbody = GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();

            InputReader = InputFactory.Create(_inputType);

            SetState(CharacterState.CanMove, 
                     CharacterState.CanRotate, 
                     CharacterState.CanPunch, 
                     CharacterState.CanInteract);
        }

        protected virtual void Update()
        {
            if (_frezeePlayer) return;

            HandleMovement();
            HandlePunch();
            HandleRotation();
            HandleInteraction();
        }

        protected virtual void HandlePunch()
        {
            if (!CanPunch)
            {
                return;
            }

            if (InputReader.XDown())
            {
                Animator.SetTrigger(SharedAnimatorParameters.PUNCH_ID);
            }
        }

        protected virtual void HandleMovement()
        {
            //translation
            _movement = Camera.main.transform.right   * InputReader.LeftAnalogStickHorizontal() +
                        Camera.main.transform.forward * InputReader.LeftAnalogStickVertical();
            _movement.y = 0; // Forces the Y axis to be 0

            IsMoving = _movement != Vector3.zero && CanMove;

            Animator.SetBool(SharedAnimatorParameters.WALKING_ID, IsMoving);

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
            Vector3 rotationDirection = Camera.main.transform.right * InputReader.RightAnalogStickHorizontal() +
                        Camera.main.transform.forward * InputReader.RightAnalogStickVertical();

            rotationDirection.y = 0;

            if (Mathf.Abs(rotationDirection.x) >= .2f || Mathf.Abs(rotationDirection.z) >= .2f)
            {
                _endRotation = Quaternion.LookRotation(rotationDirection);
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
            
            if (InputReader.ADown())
            {
                _interactionHandler.Execute();
            }
        }
    }
}