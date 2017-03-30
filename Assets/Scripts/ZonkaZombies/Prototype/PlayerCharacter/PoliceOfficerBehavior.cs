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
        private float _characterRotationLerpT = 0.2f;

        private Vector3 _movement;
        private InputReader _inputReader;
        private Rigidbody _characteRigidbody;

        private void Awake()
        {
            _characteRigidbody = GetComponent<Rigidbody>();
            _inputReader = InputFactory.Create(_inputType);
        }

        private void Update()
        {
            if (_inputReader.RightTriggerDown())
            {
                Instantiate(_bulletPrefab, _gunTransform.position, _gunTransform.rotation);
            }

            _inputReader.Update();
        }

        private void FixedUpdate()
        {
            //translation
            _movement.Set(_inputReader.LeftAnalogStickHorizontal(), 0f, _inputReader.LeftAnalogStickVertical());
            _movement = _movement.normalized * _speed * Time.deltaTime;
            _characteRigidbody.MovePosition(this.transform.position + _movement);

            //rotation
            Vector3 rotationDirection = new Vector3(_inputReader.RightAnalogStickHorizontal() * -1, 0f, _inputReader.RightAnalogStickVertical() * -1);
            //avoids Debug.Log() messages when call Quaternion.LookRotation(rotationDirection) with rotationDirection == Vector3.zero
            if (rotationDirection == Vector3.zero) return;
            Quaternion rotation = Quaternion.LookRotation(rotationDirection);
            //if no rotation does not neet to call _characteRigidbody.MoveRotation()
            if (rotation == Quaternion.identity) return;
            _characteRigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, _characterRotationLerpT));
        }
    }
}