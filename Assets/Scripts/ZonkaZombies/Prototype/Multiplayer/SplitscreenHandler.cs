using UnityEngine;

namespace ZonkaZombies.Prototype.Multiplayer
{
    public sealed class SplitscreenHandler : MonoBehaviour
    {
        #region Unity inspector values
        [SerializeField, Tooltip("Distance away from the player(s) that the camera(s) should be")]
        private float cameraDistance = 30.0f;

        [SerializeField, Tooltip("How far apart players must be before splitscreen kicks in")]
        private float triggerDistance = 10.0f;

        [SerializeField, Tooltip("The scene's primary camera")]
        private Camera primaryCamera;

        [SerializeField, Tooltip("A Transform that moves with player 1")]
        private Transform player1;

        [SerializeField, Tooltip("A Transform that moves with player 2")]
        private Transform player2;

        [SerializeField, Tooltip("Whether to show a separation stripe when the screen splits")]
        private bool showSeparator;
        #endregion

        #region Private variables

        public AudioListener _audioListener;

        /// <summary>
        /// 2d camera displacement from player to camera target (the same as <c>cameraDisplacement3d</c> with <c>y</c> set to 0)
        /// </summary>
        private Vector3 _cameraDisplacement2D;

        /// <summary>
        /// The rotation both cameras use
        /// </summary>
        /// <remarks>This is set up based on the values set for cameraRotation variable</remarks>
        private Quaternion _cameraQuaternion;

        /// <summary>
        /// The point in space that camera 1 looks at
        /// </summary>
        private Vector3 _cameraTarget1;

        /// <summary>
        /// The point in space that camera 2 looks at
        /// </summary>
        private Vector3 _cameraTarget2;

        /// <summary>
        /// The position between both players
        /// </summary>
        private Vector3 _centralPosition;

        /// <summary>
        /// The distance between both players
        /// </summary>
        private Vector3 _distanceBetweenPlayers;

        /// <summary>
        /// Track whether the separator stripe has necessary components
        /// </summary>
        private bool _isSeparatorUsable;

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
        /// The calculated scale for the splitscreen mask
        /// </summary>
        private Vector3 _maskScale;

        /// <summary>
        /// The Transform on the splitscreen mask
        /// </summary>
        private Transform _maskTransform;

        /// <summary>
        /// 2d distance (in screen space) between the player and its camera's target
        /// </summary>
        private Vector2 _screenDisplacement2D;

        /// <summary>
        /// Secondary camera
        /// </summary>
        /// <remarks>This is automatically created as a slightly altered clone of the primary camera</remarks>
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
        #endregion

        #region Public accessors
/// <summary>
        /// Gets a value indicating whether the screen is currently split, showing half of the screen for each player
        /// </summary>
        public bool IsSplitscreenOn { get; private set; }
        /// <summary>
        /// Gets a value representing an active player: p1 if it's assigned; p2 if it's assigned and p1 isn't; null otherwise.
        /// </summary>
        public Transform MainPlayer
        {
            get
            {
                if (this.player1)
                {
                    return this.player1;
                }
                else if (this.player2)
                {
                    return this.player2;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the number of players being tracked
        /// </summary>
        public int NumPlayers
        {
            get
            {
                return (this.player1 ? 1 : 0) + (this.player2 ? 1 : 0);
            }
        }
        #endregion

        #region Unity methods
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            this.Initialize();
            InitializeCameras();
        }

        /// <summary>
        /// This is called every frame after all Update methods have been called
        /// </summary>
        private void LateUpdate()
        {
            // There must be at least one player assigned for the camera(s) to have something to track
            if (this.NumPlayers == 0)
            {
                Debug.LogWarning("SplitscreenHandler: No players are assigned. There is nothing to do.");
                return;
            }

            // Find the average location of all tracked players
            this.SetCentralPosition();

            // Place the AudioListener in the central position
            this._audioListener.transform.position = this._centralPosition;

            // Determine if players are far enough apart to use splitscreen
            this._distanceBetweenPlayers = this._centralPosition - this.MainPlayer.position;
            this.HandleSplitscreenState();

            // Position camera(s)
            if (this.IsSplitscreenOn)
            {
                Color ctx = _separatorMaterial.color;
                ctx.a = ToShaderAlphaValue();
                _separatorMaterial.color = ctx;

                // Adjust displacement to be in the direction of the central point but not at it
                this._cameraDisplacement2D = this._distanceBetweenPlayers.normalized * this.triggerDistance;

                // Aim cameras at players
                this._cameraTarget1 = this.player1.position + this._cameraDisplacement2D;
                this._cameraTarget2 = this.player2.position - this._cameraDisplacement2D;
                this.MoveCamera(this.primaryCamera, this._cameraTarget1);
                this.MoveCamera(this._secondaryCamera, this._cameraTarget2);

                // Position the splitscreen mask in front of the second camera
                this.PositionSplitscreenMask(this._secondaryCamera, this.player2.position, this.player2.position + this._cameraDisplacement2D);
                this._separatorRenderer.enabled = this._isSeparatorUsable && this.showSeparator;
            }
            else
            {
                this.MoveCamera(this.primaryCamera, this.MainPlayer.position + this._distanceBetweenPlayers);
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Set centralPosition to the 3D central point in the world equidistant from both players
        /// </summary>
        /// <remarks>If there is only one player, this will return that player's position</remarks>
        private void SetCentralPosition()
        {
            this._centralPosition = Vector3.zero;

            if (this.player1)
            {
                this._centralPosition += this.player1.position;
            }

            if (this.player2)
            {
                this._centralPosition += this.player2.position;
            }

            this._centralPosition /= this.NumPlayers;
        }


/// <summary>
        /// Validates that required member variables are set and assigns values to the class's private members
        /// Sets IsInitialized to True if all expected data are present; False otherwise
        /// </summary>
        private void Initialize()
        {
            if (!primaryCamera)
            {
                Debug.LogError("primaryCamera is not set!");
            }

            _cameraQuaternion = primaryCamera.transform.rotation;
            this._maskScale = Vector3.one;

            // Put the mask just inside the view frustum
            this._maskOffset = this.primaryCamera.nearClipPlane + 0.1f;

            AudioListener cameraListener = this.primaryCamera.GetComponent<AudioListener>();
            if (cameraListener)
            {
                Debug.Log("Primary Camera has an AudioListener. It will be removed.");
                Destroy(cameraListener);
            }

            // Set up the splitscreen mask and separator
            this._isSeparatorUsable = false;
            this._splitscreenMask = GetComponentInChildren<MeshRenderer>();
            if (!_splitscreenMask)
            {
                Debug.LogError("No MeshRenderer mask found in Magic Splitscreen's children.");
            }
            else
            {
                this._maskLayer = this._splitscreenMask.gameObject.layer;
                this._maskTransform = this._splitscreenMask.transform;

                MeshRenderer[] renderers = this._splitscreenMask.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mr in renderers)
                {
                    if (mr != this._splitscreenMask)
                    {
                        this._separatorRenderer = mr;
                        break;
                    }
                }

                this._separatorMaterial = this._separatorRenderer.material;
                this._isSeparatorUsable = true;
            }
        }

        /// <summary>
        /// Initializes the primary camera and creates the secondary camera
        /// </summary>
        private void InitializeCameras()
        {
            // Clone the primary camera
            this._secondaryCamera = GameObject.Instantiate(this.primaryCamera);
            this._secondaryCamera.transform.parent = this.transform;
            this._secondaryCamera.clearFlags = CameraClearFlags.Depth;

            SetCentralPosition();

            MoveCamera(this.primaryCamera, _centralPosition);

            DisableSplitscreen();
        }

        /// <summary>
        /// Move a camera to look at a specified position
        /// </summary>
        /// <param name="camera">The camera to move</param>
        /// <param name="targetPos">The position for that camera to look at</param>
        /// <remarks>This is the place to add specialized camera movement behavior if you want it.</remarks>
        private void MoveCamera(Camera camera, Vector3 targetPos)
        {
            camera.transform.localRotation = this._cameraQuaternion;
            camera.transform.position = targetPos - (camera.transform.forward * this.cameraDistance);
        }

        /// <summary>
        /// Positions the splitscreen mask to cover a player's half of the screen
        /// </summary>
        /// <param name="camera">The camera to position the mask in front of</param>
        /// <param name="playerPos">The position of the player to place the mask over</param>
        /// <param name="targetPos">A 3D position that should be considered along the line from the player toward of the center of the screen</param>
        private void PositionSplitscreenMask(Camera camera, Vector3 playerPos, Vector3 targetPos)
        {
            // Resize the mask to cover the proper amount of the screen. It just needs to be long enough to go past the
            // ends of a diagonal across the screen. Making it square makes for fewer calculations to get it big enough  
            if (camera.orthographic)
            {
                if (camera.aspect >= 1.0f)
                {
                    this._maskScale.x = this._maskScale.y = camera.orthographicSize * 2.83f /* 2√2 */ * camera.aspect;
                }
                else
                {
                    this._maskScale.x = this._maskScale.y = camera.orthographicSize * 2.83f /* 2√2 */;
                }
            }
            else
            {
                if (camera.aspect >= 1.0f)
                {
                    this._maskScale.x = this._maskScale.y = 2.83f /* 2√2 */ * this._maskOffset * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * camera.aspect;
                }
                else
                {
                    this._maskScale.x = this._maskScale.y = 2.83f /* 2√2 */ * this._maskOffset * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) / camera.aspect;
                }
            }

            this._maskTransform.localScale = this._maskScale;

            // Project the two points onto the camera's 2D view
            this._screenDisplacement2D = camera.WorldToScreenPoint(playerPos) - camera.WorldToScreenPoint(targetPos);

            // Align the splitscreen mask with the camera and rotate it based on the split angle
            this._maskTransform.rotation = camera.transform.rotation;
            this._maskTransform.Rotate(this._maskTransform.forward, Mathf.Atan2(this._screenDisplacement2D.y, this._screenDisplacement2D.x) * Mathf.Rad2Deg, Space.World);

            // Place the mask in front of the camera, far enough to the side to only conceal half of the screen
            this._maskTransform.position = camera.transform.position + (camera.transform.forward * this._maskOffset) + (this._maskTransform.right * this._splitscreenMask.transform.lossyScale.x * 0.5f);
        }
        
        private void HandleSplitscreenState()
        {
            if (IsSplitscreenOn)
            {
                if (_distanceBetweenPlayers.sqrMagnitude < triggerDistance * triggerDistance)
                {
                    DisableSplitscreen();
                }
            }
            else
            {
                if (_distanceBetweenPlayers.sqrMagnitude > triggerDistance * triggerDistance)
                {
                    EnableSplitscreen();
                }
            }
        }
        
        private void EnableSplitscreen()
        {
            // Activate the splitscreen components
            _secondaryCamera.gameObject.SetActive(true);
            _maskTransform.gameObject.SetActive(true);

            // Position the new camera
            _secondaryCamera.transform.position = primaryCamera.transform.position;
            _secondaryCamera.transform.rotation = _cameraQuaternion;

            // Turn off culling of the splitscreen mask layer for the main camera
            primaryCamera.cullingMask &= ~(1 << _maskLayer);

            IsSplitscreenOn = true;
        }

        /// <summary>
        /// Disable splitscreen
        /// </summary>
        private void DisableSplitscreen()
        {
            // Just turn everything off
            this.IsSplitscreenOn = false;
            this._secondaryCamera.gameObject.SetActive(false);
            this._maskTransform.gameObject.SetActive(false);
            this._separatorRenderer.enabled = false;
        }
        #endregion

        private float ToShaderAlphaValue()
        {
            float minFadeDistance = triggerDistance * triggerDistance;
            float maxFadeDistance = minFadeDistance * 3;
            float result = Mathf.InverseLerp(minFadeDistance, maxFadeDistance, _distanceBetweenPlayers.sqrMagnitude);
            return result;
        }
    }
}
