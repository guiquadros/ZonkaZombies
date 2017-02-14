using UnityEngine;

namespace Assets.Scripts.Cam
{
    /// <summary>
    /// Script que permite fazer com que a câmera siga um determinado objeto.
    /// É possível alterar o objeto dinamicamente apenas alterando o atributo
    /// target;
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class MoveWithObject : MonoBehaviour {
        #region EDITOR VARIABLES
        [Header("Configuracoes")]
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float camMovementVel;

        [Header("Hack")]
        [SerializeField]
        private bool followPlayer;
        [SerializeField]
        private bool stopFollowing;
        #endregion

        #region PRIVATE VARIABLES
        //Private Variables
        private Transform cameraTransform;
        private Vector3 backward;
        private bool isFollowing;
        #endregion

        #region MONOBEHAVIOUR METHODS
        private void Awake()
        {
            cameraTransform = GetComponent<Transform>();

            backward = -cameraTransform.forward * 40f;
        }

        private void Start()
        {
            Stop();
        }

        private void Update()
        {
            if (followPlayer)
            {
                followPlayer = false;
                Follow();
            } else if (stopFollowing)
            {
                stopFollowing = false;
                Stop();
            }

        }

        /// <summary>
        /// Use o método LateUpdate para mover a câmera após o personagem ter
        /// se movido, causando um efeito de delay.
        /// </summary>
        private void LateUpdate()
        {
            if (!isFollowing)
                return;

            cameraTransform.position = Vector3.MoveTowards(
                cameraTransform.position, 
                target.position + backward,
                camMovementVel*Time.deltaTime);
        }
        #endregion

        #region MOVEWITHOBJECT METHODS
        public void Follow()
        {
            if (!target)
                return;

            isFollowing = true;
        }

        public void Stop()
        {
            isFollowing = false;
        }
        #endregion
    }
}
