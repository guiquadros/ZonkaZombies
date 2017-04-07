using UnityEngine;
using ZonkaZombies.Input;

namespace ZonkaZombies.Prototype.PlayerCharacter
{
    public class PoliceOfficerBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 6f;

        [SerializeField]
        private InputType _inputType = InputType.Controller1;

        [SerializeField]
        private GameObject _bulletPrefab;

        [SerializeField]
        private Transform _gunTransform;

        [SerializeField]
        private Transform _bodyTransform;

        [SerializeField]
        private float _rotationCounter = 0.0f;
        private float _rotationDelta = 0.0f;

        [SerializeField]
        [Range(0, 1440)]
        private float _angularSpeed = 720.0f;

        private Vector3 _movement;
        private InputReader _inputReader;
        private Rigidbody _characteRigidbody;
        private Quaternion _startRotation = Quaternion.identity;
        private Quaternion _endRotation = Quaternion.identity;

        private void Awake()
        {
            _characteRigidbody = GetComponent<Rigidbody>();
            _inputReader = InputFactory.Create(_inputType);
        }

        private void Update()
        {
            HandleGunFire();
            HandleMovement();
            HandleRotation();

            _inputReader.Update();
        }

        private void HandleMovement()
        {
            //translation
            _movement.Set(_inputReader.LeftAnalogStickHorizontal(), 0f, _inputReader.LeftAnalogStickVertical());
            _movement = _movement.normalized * _speed * Time.deltaTime;
            _characteRigidbody.MovePosition(this.transform.position + _movement);
        }

        private void HandleRotation()
        {
            //rotation
            Vector3 rotationDirection = new Vector3(-_inputReader.RightAnalogStickHorizontal(), 0f, -_inputReader.RightAnalogStickVertical());

            if (rotationDirection != Vector3.zero) 
            {
                _rotationCounter = 0.0f;
                _startRotation = transform.rotation;
                _endRotation = Quaternion.LookRotation(rotationDirection);

                float startY = _startRotation.eulerAngles.y;
                float endY = _endRotation.eulerAngles.y;

                _rotationDelta = Mathf.Abs(startY - endY);
                float result = Mathf.Abs(startY - (endY - 360f));

                if (result < _rotationDelta)
                    _rotationDelta = result;
                result = Mathf.Abs(startY - (endY + 360f));

                if (result < _rotationDelta)
                    _rotationDelta = result;

                if (_rotationDelta <= 0.01f) return;
            }

            _rotationCounter += Time.deltaTime * _angularSpeed / _rotationDelta;
            transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, _rotationCounter);
        }

        private void HandleGunFire()
        {
            if (_inputReader.RightTriggerDown())
            {
                Instantiate(_bulletPrefab, _gunTransform.position, _gunTransform.rotation);
            }
        }
    }
}