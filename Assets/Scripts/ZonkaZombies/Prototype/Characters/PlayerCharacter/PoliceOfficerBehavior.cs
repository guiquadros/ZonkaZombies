using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Prototype.Characters.Enemy;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class PoliceOfficerBehavior : PlayerCharacterBehavior
    {
        [SerializeField]
        private float _speed = 6f;

        [SerializeField]
        private InputType _inputType = InputType.Controller1;

        [SerializeField]
        [Range(0, 1440)]
        private float _angularSpeed = 720.0f;

        [SerializeField]
        private GameObject _enemies;

        private Vector3 _movement;
        private Rigidbody _characteRigidbody;
        private Quaternion _endRotation = Quaternion.identity;
        
        protected override void Awake()
        {
            base.Awake();

            _characteRigidbody = GetComponent<Rigidbody>();
            InputReader = InputFactory.Create(_inputType);
        }

        private void Update()
        {
            HandlePunch();
            HandleMovement();
            HandleRotation();


            //TODO: find a better way to do this in terms of performance. The PoliceOfficerBehavior class is not the best place to put the win condition (we should have a GameManager class).
            if (_enemies != null)
            {
                EnemyBehavior[] childrenComponents = _enemies.GetComponentsInChildren<EnemyBehavior>();

                if (childrenComponents.Length == 0)
                {
                    SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
                }
            }
        }

        private void LateUpdate()
        {
            InputReader.Update();
        }
        
        private void HandlePunch()
        {
            if (InputReader.XDown() || UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                DoPunch();
            }
        }

        private void HandleMovement()
        {
            //translation
            _movement.Set(InputReader.LeftAnalogStickHorizontal(), 0f, InputReader.LeftAnalogStickVertical());
            _movement = _movement.normalized * _speed * Time.deltaTime;
            _characteRigidbody.MovePosition(this.transform.position + _movement);
        }

        private void HandleRotation()
        {
            //rotation
            Vector3 rotationDirection = new Vector3(-InputReader.RightAnalogStickHorizontal(), 0f, -InputReader.RightAnalogStickVertical());

            if (Mathf.Abs(rotationDirection.x) >= .2f || Mathf.Abs(rotationDirection.z) >= .2f)
            {
                _endRotation = Quaternion.LookRotation(rotationDirection);
            }

            Vector3 tempRotation = transform.rotation.eulerAngles;
            tempRotation.y = Mathf.MoveTowardsAngle(tempRotation.y, _endRotation.eulerAngles.y, _angularSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(tempRotation);
        }
    }
}