using UnityEngine;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;

namespace ZonkaZombies.Multiplayer
{
    public sealed class SplitscreenHandler : MonoBehaviour
    {
        [SerializeField, Tooltip("The scene's primary camera")]
        private Camera _primaryCamera;
        [SerializeField, Tooltip("Distance away from the player(s) that the camera(s) should be")]
        private float _cameraDistance = 30.0f;
        [SerializeField, Tooltip("How far apart players must be before splitscreen kicks in")]
        private float _triggerDistance = 9f;
        [SerializeField]
        private AudioListener _audioListener;
        [SerializeField, Tooltip("A Transform that moves with player 1")]
        public Transform Player1;
        [SerializeField, Tooltip("A Transform that moves with player 2")]
        public Transform Player2;

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
        private Transform _maskTransform, _maskTransform2;
        private Quaternion _defaultCameraQuaternion;
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
        public Transform MainPlayer
        {
            get
            {
                return Player1 ? Player1 : Player2 ? Player2 : null;
            }
        }

        /// <summary>
        /// Gets the number of players being tracked
        /// </summary>
        public int NumPlayers
        {
            get
            {
                return (Player1 ? 1 : 0) + (Player2 ? 1 : 0);
            }
        }

        private void OnEnable()
        {
            MessageRouter.AddListener<OnPlayerHasBornMessage>(OnPlayerMessageCallback);
            MessageRouter.AddListener<OnPlayerSpawnMessage>(OnPlayerMessageCallback);
            MessageRouter.AddListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
        }
        
        private void OnDisable()
        {
            MessageRouter.RemoveListener<OnPlayerHasBornMessage>(OnPlayerMessageCallback);
            MessageRouter.RemoveListener<OnPlayerSpawnMessage>(OnPlayerMessageCallback);
            MessageRouter.RemoveListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
        }

        private void Awake()
        {
            this.Initialize();
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
            _distanceBetweenPlayers = _centralPosition - MainPlayer.position;

            if (IsSplitscreenOn)
            {
                // Set up the alpha value of the separator stripe
                float minFadeDistance = _triggerDistance * _triggerDistance;
                float maxFadeDistance = minFadeDistance * 3;
                Color tempColor = _separatorMaterial.color;
                tempColor.a = Mathf.InverseLerp(minFadeDistance, maxFadeDistance, _distanceBetweenPlayers.sqrMagnitude);
                _separatorMaterial.color = tempColor;

                // Adjust displacement to be in the direction of the central point, but not at it
                _cameraDisplacement2D = _distanceBetweenPlayers.normalized * _triggerDistance;

                // Aim cameras at players
                SetCameraPos(_primaryCamera,   Player1.position + _cameraDisplacement2D);
                SetCameraPos(_secondaryCamera, Player2.position - _cameraDisplacement2D);

                // Position the splitscreen mask in front of the second camera
                SetSplitscreenMaskPos(_secondaryCamera, Player2.position, Player2.position + _cameraDisplacement2D, _maskTransform);

                if (_distanceBetweenPlayers.sqrMagnitude < _triggerDistance * _triggerDistance)
                {
                    DisableSplitscreen();
                }
            }
            else
            {
                SetCameraPos(_primaryCamera, MainPlayer.position + _distanceBetweenPlayers);

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
                tempCentralPos += Player1.position;
                tempCentralPos += Player2.position;
                tempCentralPos /= NumPlayers;
                return tempCentralPos;
            }
            
            return MainPlayer ? MainPlayer.position : Vector3.zero;
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
                _maskTransform  = _splitscreenMask.transform;
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
            this._secondaryCamera = Instantiate(this._primaryCamera);

            _secondaryCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

            this._secondaryCamera.transform.parent = this.transform;

            GetCentralPosition();

            SetCameraPos(this._primaryCamera, _centralPosition);

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
            target.transform.position = targetPos - (target.transform.forward * _cameraDistance);
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
                _maskScale.x = _maskScale.y = 2.83f /* 2√2 */ * _maskOffset * Mathf.Tan(target.fieldOfView * 0.5f * Mathf.Deg2Rad) * target.aspect;
            }
            else
            {
                _maskScale.x = _maskScale.y = 2.83f /* 2√2 */ * _maskOffset * Mathf.Tan(target.fieldOfView * 0.5f * Mathf.Deg2Rad) / target.aspect;
            }

            maskTranform.localScale = _maskScale;

            // Project the two points onto the camera's 2D view
            _screenDisplacement2D = target.WorldToScreenPoint(playerPos) - target.WorldToScreenPoint(targetPos);

            // Align the splitscreen mask with the camera and rotate it based on the split angle
            maskTranform.rotation = target.transform.rotation;
            maskTranform.Rotate(maskTranform.forward, Mathf.Atan2(_screenDisplacement2D.y, _screenDisplacement2D.x) * Mathf.Rad2Deg, Space.World);

            // Place the mask in front of the camera, far enough to the side to only conceal half of the screen
            maskTranform.position = target.transform.position + (target.transform.forward * _maskOffset) + (maskTranform.right * _splitscreenMask.transform.lossyScale.x * 0.5f);
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

        private void OnPlayerMessageCallback(BasePlayerMessage message)
        {
            if (message.Player.IsFirstPlayer)
            {
                Player1 = message.Player.transform;
            }
            else
            {
                Player2 = message.Player.transform;
            }
        }

        private void OnPlayerDeadCallback(OnPlayerDeadMessage message)
        {
            if (message.Player.IsFirstPlayer)
            {
                Player1 = null;
            }
            else
            {
                Player2 = null;
            }

            DisableSplitscreen();
        }

#endregion
    }
}
