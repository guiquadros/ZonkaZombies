using UnityEngine;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;

namespace ZonkaZombies.Controllers
{
    public class MainMenuCameraController : MonoBehaviour
    {
        private Vector3 _startLocalPosition;
        private Vector3 _targetLocalPosition;
        private Quaternion _startRotation;
        private Quaternion _targetRotation;        

        private float _transitionCounter;
        private float _transitionDuration;

        private bool _isLerping;
        
        private void OnEnable()
        {
            MessageRouter.AddListener<MoveCameraMessage>(OnMoveCameraMessageReceived);
        }

        private void OnDisable()
        {
            MessageRouter.RemoveListener<MoveCameraMessage>(OnMoveCameraMessageReceived);
        }

        private void OnMoveCameraMessageReceived(MoveCameraMessage msg)
        {
            // Reset class's variables
            _transitionCounter = 0;
            _isLerping = true;

            // Track current Transform
            _startLocalPosition = transform.position;
            _startRotation = transform.rotation;

            _targetLocalPosition = msg.Position;
            _targetRotation      = msg.Rotation;
            _transitionDuration  = msg.Duration;
        }

        private void LateUpdate()
        {
            if (!_isLerping)
            {
                return;
            }

            _transitionCounter += Time.deltaTime;

            float step = Mathf.Clamp01(_transitionCounter / _transitionDuration);
            
            // Do a lerp on both position and rotation
            transform.localPosition = Vector3.Lerp(_startLocalPosition, _targetLocalPosition, step);
            transform.rotation      = Quaternion.Lerp(_startRotation, _targetRotation, step);

            _isLerping = step < 1;
        }
    }
}