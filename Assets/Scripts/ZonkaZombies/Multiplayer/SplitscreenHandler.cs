using UnityEngine;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Managers;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.Util;

namespace ZonkaZombies.Multiplayer
{
    public sealed class SplitscreenHandler : MonoBehaviour
    {
        [SerializeField, Tooltip("The scene's primary camera")]
        private Camera _primaryCamera;

        [SerializeField, Tooltip("Distance away from the player(s) that the camera(s) should be")]
        private float _cameraDistance = 30.0f;

        [SerializeField, Tooltip("How far apart players must be before splitscreen kicks in")]
        private float _triggerDistance = 12f;

        [SerializeField]
        private AudioListener _audioListener;

        private Player _firstPlayer;
        private Player _secondPlayer;

        /// <summary>
        /// 2d camera displacement from player to camera target (the same as <c>cameraDisplacement3d</c> with <c>y</c> set to 0)
        /// </summary>
        private Vector3 _cameraDisplacement2D;

        /// <summary>
        /// The position between both players
        /// </summary>
        private Vector3 _centralPosition;

        /// <summary>
        /// The distance between both players
        /// </summary>
        private Vector3 _distanceBetweenPlayers;

        /// <summary>
        /// The calculated scale for the splitscreen mask
        /// </summary>
        private Vector3 _maskScale;

        /// <summary>
        /// 2d distance (in screen space) between the player and its camera's target
        /// </summary>
        private Vector2 _screenDisplacement2D;

        /// <summary>
        /// The layer for the splitscreen mask
        /// </summary>
        private int _maskLayer;

        /// <summary>
        /// Distance in front of Camera 2 that the splitscreen mask should be
        /// </summary>
        /// <remarks>This is automatically set to be just past the near plane inside that camera's viewing frustum</remarks>
        private float _maskOffset;

        /// <summary>
        /// The Transform on the splitscreen mask
        /// </summary>
        private Transform _maskTransform,
            _maskTransform2;

        private Quaternion _defaultCameraQuaternion;
        [SerializeField]
        private Camera _secondaryCamera;

        /// <summary>
        /// Material on the separator stripe
        /// </summary>
        private Material _separatorMaterial;

        /// <summary>
        /// MeshRenderer for the separator stripe
        /// </summary>
        private MeshRenderer _separatorRenderer;

        /// <summary>
        /// MeshRenderer for the splitscreen mask
        /// </summary>
        private MeshRenderer _splitscreenMask;

        public bool IsSplitscreenOn { get; private set; }
        /// <summary>
        /// Gets a value representing an active player: p1 if it's assigned; p2 if it's assigned and p1 isn't; null otherwise.
        /// </summary>
        public Transform MainPlayerTransform
        {
            get
            {
                return _firstPlayer ? _firstPlayer.transform : _secondPlayer ? _secondPlayer.transform : null;
            }
        }

        /// <summary>
        /// Gets the number of players being tracked
        /// </summary>
        public int NumPlayers
        {
            get { return (_firstPlayer ? 1 : 0) + (_secondPlayer ? 1 : 0); }
        }

        private void OnEnable()
        {
            MessageRouter.AddListener<OnPlayerHasBornMessage>(OnPlayerHasBornMessageCallback);
            MessageRouter.AddListener<OnPlayerSpawnMessage>(OnPlayerHasBornMessageCallback);
            MessageRouter.AddListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);

            SceneController.Instance.AfterSceneLoad += OnAfterSceneLoad;
        }

        private void OnDisable()
        {
            MessageRouter.RemoveListener<OnPlayerHasBornMessage>(OnPlayerHasBornMessageCallback);
            MessageRouter.RemoveListener<OnPlayerSpawnMessage>(OnPlayerHasBornMessageCallback);
            MessageRouter.RemoveListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);

            SceneController.Instance.AfterSceneLoad -= OnAfterSceneLoad;
        }

        private void Awake()
        {
            Initialize();
            InitializeCameras();
        }

        private void LateUpdate()
        {
            // There must be at least one player assigned for the camera(s) to have something to track
            if (NumPlayers == 0)
            {
                Debug.LogWarning("SplitscreenHandler: No players are assigned. There is nothing to do.");
                return;
            }

            _centralPosition = GetCentralPosition();
            _audioListener.transform.position = _centralPosition;
            _distanceBetweenPlayers = _centralPosition - MainPlayerTransform.position;

            float distanceBetweenPlayersSqr = _distanceBetweenPlayers.sqrMagnitude;
            float minFadeDistance = _triggerDistance * _triggerDistance;
            float maxFadeDistance = minFadeDistance * 3;

            if (IsSplitscreenOn)
            {
                if (_firstPlayer == null || _secondPlayer == null)
                {
                    DisableSplitscreen();
                    return;
                }

                // Set up the alpha value of the separator stripe
                Color tempColor = _separatorMaterial.color;
                tempColor.a = Mathf.InverseLerp(minFadeDistance, maxFadeDistance, distanceBetweenPlayersSqr);
                _separatorMaterial.color = tempColor;

                // Adjust displacement to be in the direction of the central point, but not at it
                _cameraDisplacement2D = _distanceBetweenPlayers.normalized * _triggerDistance;

                // Aim cameras at players
                SetCameraPos(_primaryCamera, _firstPlayer.transform.position + _cameraDisplacement2D);
                SetCameraPos(_secondaryCamera, _secondPlayer.transform.position - _cameraDisplacement2D);

                // Position the splitscreen mask in front of the second camera
                SetSplitscreenMaskPos(_secondaryCamera, _secondPlayer.transform.position, _secondPlayer.transform.position + _cameraDisplacement2D,
                    _maskTransform);

                if (_distanceBetweenPlayers.sqrMagnitude < _triggerDistance * _triggerDistance)
                {
                    DisableSplitscreen();
                }
            }
            else
            {
                SetCameraPos(_primaryCamera, MainPlayerTransform.position + _distanceBetweenPlayers);

                if (_distanceBetweenPlayers.sqrMagnitude > _triggerDistance * _triggerDistance)
                {
                    EnableSplitscreen();
                }
            }
        }

        /// <summary>
        /// Set centralPosition to the 3D central point in the world equidistant from both players
        /// </summary>
        /// <remarks>If there is only one player, this will return that player's position</remarks>
        public Vector3 GetCentralPosition()
        {
            if (NumPlayers == 2)
            {
                Vector3 tempCentralPos = Vector3.zero;
                tempCentralPos += _firstPlayer.transform.position;
                tempCentralPos += _secondPlayer.transform.position;
                tempCentralPos /= NumPlayers;
                return tempCentralPos;
            }

            return MainPlayerTransform ? MainPlayerTransform.position : Vector3.zero;
        }

        private void Initialize()
        {
            _defaultCameraQuaternion = _primaryCamera.transform.rotation;
            _maskScale = Vector3.one;

            // Put the mask just inside the view frustum
            _maskOffset = _primaryCamera.nearClipPlane + 0.1f;

            AudioListener cameraListener = _primaryCamera.GetComponent<AudioListener>();
            if (cameraListener)
            {
                Debug.Log("Primary Camera has an AudioListener. It will be removed.");
                Destroy(cameraListener);
            }

            // Set up the splitscreen mask and separator
            _splitscreenMask = GetComponentInChildren<MeshRenderer>();
            if (!_splitscreenMask)
            {
                Debug.LogError("No MeshRenderer mask found in Magic Splitscreen's children.");
            }
            else
            {
                _maskLayer = _splitscreenMask.gameObject.layer;
                _maskTransform = _splitscreenMask.transform;
                _maskTransform2 = _splitscreenMask.transform;

                MeshRenderer[] renderers = _splitscreenMask.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mr in renderers)
                {
                    if (mr != _splitscreenMask)
                    {
                        _separatorRenderer = mr;
                        break;
                    }
                }

                _separatorMaterial = _separatorRenderer.material;
            }
        }

        /// <summary>
        /// Initializes the primary camera and creates the secondary camera
        /// </summary>
        private void InitializeCameras()
        {
            // Clone the primary camera
            _secondaryCamera.clearFlags = CameraClearFlags.Skybox;
            _secondaryCamera.transform.parent = transform;
            SetCameraPos(_primaryCamera, _centralPosition);
            DisableSplitscreen();
        }
        
        /// <summary>
        /// Move a camera to look at a specified position
        /// </summary>
        /// <param name="target">The camera to move</param>
        /// <param name="targetPos">The position for that camera to look at</param>
        /// <remarks>This is the place to add specialized camera movement behavior if you want it.</remarks>
        private void SetCameraPos(Camera target, Vector3 targetPos)
        {
            target.transform.localRotation = _defaultCameraQuaternion;
            target.transform.position = Vector3.Slerp(target.transform.position, targetPos - (target.transform.forward * _cameraDistance), Time.deltaTime * 5);
        }

        /// <summary>
        /// Positions the splitscreen mask to cover a player's half of the screen
        /// </summary>
        /// <param name="target">The camera to position the mask in front of</param>
        /// <param name="playerPos">The position of the player to place the mask over</param>
        /// <param name="targetPos">A 3D position that should be considered along the line from the player toward of the center of the screen</param>
        private void SetSplitscreenMaskPos(Camera target, Vector3 playerPos, Vector3 targetPos, Transform maskTranform)
        {
            if (target.aspect >= 1.0f)
            {
                _maskScale.x = _maskScale.y = 2.83f /* 2√2 */ * _maskOffset *
                                              Mathf.Tan(target.fieldOfView * 0.5f * Mathf.Deg2Rad) * target.aspect;
            }
            else
            {
                _maskScale.x = _maskScale.y = 2.83f /* 2√2 */ * _maskOffset *
                                              Mathf.Tan(target.fieldOfView * 0.5f * Mathf.Deg2Rad) / target.aspect;
            }

            maskTranform.localScale = _maskScale;

            // Project the two points onto the camera's 2D view
            _screenDisplacement2D = target.WorldToScreenPoint(playerPos) - target.WorldToScreenPoint(targetPos);

            // Align the splitscreen mask with the camera and rotate it based on the split angle
            maskTranform.rotation = target.transform.rotation;
            maskTranform.Rotate(maskTranform.forward,
                Mathf.Atan2(_screenDisplacement2D.y, _screenDisplacement2D.x) * Mathf.Rad2Deg, Space.World);

            // Place the mask in front of the camera, far enough to the side to only conceal half of the screen
            maskTranform.position = target.transform.position + (target.transform.forward * _maskOffset) +
                                    (maskTranform.right * _splitscreenMask.transform.lossyScale.x * 0.5f);
        }

        private void OnAfterSceneLoad()
        {
            _primaryCamera.gameObject.SetActive(true);
            MessageRouter.SendMessage(new SplitScreenCamerasInitializedMessage()
            {
                CameraTransforms = new [] { _primaryCamera.transform, _secondaryCamera.transform }
            });
        }

        private void EnableSplitscreen()
        {
            // Activate the splitscreen components
            _secondaryCamera.gameObject.SetActive(true);
            _maskTransform.gameObject.SetActive(true);
            _maskTransform2.gameObject.SetActive(true);

            // Position the new camera
            _secondaryCamera.transform.position = _primaryCamera.transform.position;
            _secondaryCamera.transform.rotation = _defaultCameraQuaternion;

            // Turn off culling of the splitscreen mask layer for the main camera
            _primaryCamera.cullingMask &= ~(1 << _maskLayer);

            IsSplitscreenOn = true;
            _separatorRenderer.enabled = true;
        }

        public void DisableSplitscreen()
        {
            // Just turn everything off
            IsSplitscreenOn = false;
            _secondaryCamera.gameObject.SetActive(false);
            _maskTransform.gameObject.SetActive(false);
            _maskTransform2.gameObject.SetActive(false);
            _separatorRenderer.enabled = false;
        }

        #region CALLBACKS

        private void OnPlayerHasBornMessageCallback(BasePlayerMessage message)
        {
            if (_firstPlayer == null)
            {
                //It really doesn't matter if this is the first of second player
                _firstPlayer = message.Player;
            }
            else
            {
                _secondPlayer = message.Player;
            }
        }

        private void OnPlayerDeadCallback(OnPlayerDeadMessage message)
        {
            if (_secondPlayer == null)
            {
                _firstPlayer = null;
            }
            else if (message.Player == _firstPlayer)
            {
                _firstPlayer = _secondPlayer;
            }

            _secondPlayer = null;

            DisableSplitscreen();
        }

        #endregion
    }
}