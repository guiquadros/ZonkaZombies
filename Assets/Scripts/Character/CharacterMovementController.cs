using System.Collections.Generic;
using Assets.Scripts.Cenario.Room.Node;
using UnityEngine;

namespace Assets.Scripts.Character
{
    /// <summary>
    /// Classe responsável por mover o personagem pelo cenário, quando um destino é definido.
    /// </summary>
    [RequireComponent(typeof(CharacterMovementHandler))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMovementController : MonoBehaviour
    {
        #region DELEGATES
        public delegate void ReachTarget();
        public delegate void ReachNode(Node node);
        public delegate void BlockedPath();
        #endregion

        #region EVENTS
        /// <summary>
        /// Delegate disponibilizado para a classe CharacterMovementHandler.
        /// </summary>
        public event ReachTarget OnReachTarget;
        public event ReachNode OnReachNode;
        #endregion

        #region EVENTS DISPATCH
        private void DispatchOnReachNode()
        {
            if (OnReachNode != null)
                OnReachNode(currentNode);
        }

        /// <summary>
        /// Despacha o evento quando o personagem chega em seu destino final.
        /// </summary>
        private void DispatchReachTarget()
        {
            if (OnReachTarget != null)
                OnReachTarget();
        }
        #endregion

        #region EDITOR VARIABLES
        /// <summary>
        /// Velocidade de movimento do personagem.
        /// </summary>
        [SerializeField]
        private float movementVelocity = 8f;
        [SerializeField]
        private float lookVelocity = 3;
        [SerializeField]
        private NodeBehaviour starterNode;
        private Node currentNode;
        #endregion

        #region PUBLIC VARIABLES
        public CharacterMovementHandler Handler { get { return handler; } }
        public bool IsMoving { get { return isMoving; } }
        #endregion

        #region PRIVATE VARIABLES
        /// <summary>
        /// Guarda referência para o Transform do objeto a fim de melhorar a 
        /// a performance.
        /// </summary>
        private new Transform transform;
        private Animator anim;
        /// <summary>
        /// Instancia para o nodo atual em que o personagem está.
        /// </summary>
        private Node currentTarget;
        private CharacterMovementHandler handler;

        //Variáveis necessárias para movimento do personagem
        private Quaternion lookRotation;
        private List<Node> path;
        private int currentPathIndex;
        private bool needToLook, isMoving, isMovementCanceled;
        #endregion

        #region MONOBEHAVIOUR METHODS
        private void Awake()
        {
            transform = GetComponent<Transform>();
            anim = GetComponent<Animator>();
            handler = GetComponent<CharacterMovementHandler>();

            currentNode = starterNode.GetNode();
        }

        /// <summary>
        /// Inicializa as dependencias
        /// </summary>
        private void Start()
        {
            isMoving = false;

            //Inicializa o Haer responsável por este controlador
            this.handler.Initialize(this);

            TeleportTo(currentNode);
        }

        /// <summary>
        /// Move o personagem para o próximo nodo, caso o mesmo exista.
        /// </summary>
        private void Update()
        {
            if (!isMoving)
                return;

            //Se o personagem chegou no nodo destino
            if (currentTarget.Behaviour.transform.position == transform.position)
            {
                DispatchOnReachNode();
                GoToNextNode();
                return;
            }

            if (needToLook)
                LookAtNode();

            MoveTo();
        }
        #endregion

        #region CHARACTERMOVEMENTCONTROLLER

        /// <summary>
        /// Move o personagem imediatamente para a posição informada.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="nodoDestino"></param>
        private void TeleportTo(Node nodoDestino)
        {
            transform.position = nodoDestino.Behaviour.transform.position;
        }

        /// <summary>
        /// Retorna o Nodo atual do personagem.
        /// </summary>
        /// <returns></returns>
        public Node GetCurrentNode()
        {
            return currentNode;
        }

        /// <summary>
        /// Move o personagem para o próximo nodo da lista, caso exista.
        /// </summary>
        private void GoToNextNode()
        {
            currentPathIndex++;

            if (isMovementCanceled || currentPathIndex == path.Count) //O personagem chegou no destino
            {
                StopMovement();
                path = null;
                DispatchReachTarget();
                return;
            }

            if (path[currentPathIndex].IsAvailable())
            {

            }

            currentTarget = path[currentPathIndex];

            currentNode = currentTarget; //Altera o nodo atual

            Vector3 posToLook = currentTarget.Behaviour.transform.position - transform.position;

            //Verifica se é necessário rotacionar o personagem para fazê-lo observar o destino
            needToLook = posToLook != Vector3.zero;

            //Cálculos necessários para se saber se o personagem precisa ser rotacionado
            lookRotation = Quaternion.Euler(Vector3.zero);

            if (needToLook)
                lookRotation = Quaternion.LookRotation(posToLook);
        }

        public void MoveTo(List<Node> generatedPath)
        {
            //Não há um próximo movimento
            if (generatedPath == null)
            {
                isMovementCanceled = true;
                
                if (!isMoving)
                    StopMovement();
                return;
            } else
            {
                path = generatedPath;
                currentPathIndex = 0;

                //Cálculos necessários para se saber se o personagem precisa ser rotacionado
                lookRotation = Quaternion.Euler(Vector3.zero);

                currentTarget = path[currentPathIndex];

                Vector3 posToLook = currentTarget.Behaviour.transform.position - transform.position;

                //Verifica se é necessário rotacionar o personagem para fazê-lo observar o destino
                needToLook = posToLook != Vector3.zero;

                if (needToLook)
                    lookRotation = Quaternion.LookRotation(posToLook);

                currentNode = currentTarget; //Altera o nodo atual

                StartMovement();
            }
        }

        /// <summary>
        /// Rotaciona o personagem, o fazendo olhar para a direção do próximo nodo.
        /// </summary>
        private void LookAtNode()
        {
            transform.rotation = Quaternion.Slerp
            (transform.rotation, lookRotation,
            Time.deltaTime * movementVelocity * lookVelocity);
        }

        /// <summary>
        /// Move o personagem até seu destino interativamente.
        /// </summary>
        private void MoveTo()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                currentTarget.Behaviour.transform.position,movementVelocity * Time.deltaTime);
        }
        #endregion

        #region CHARACTERMOVEMENTCONTROLLER ANIMATION CONTROLLER
        /// <summary>
        /// Inicia a animação WALKING do personagem.
        /// </summary>
        private void StartMovement()
        {
            isMoving = true;

            if (anim != null)
                anim.SetBool("andar", isMoving);
        }

        /// <summary>
        /// Inicia a animação IDLE do personagem.
        /// </summary>
        private void StopMovement()
        {
            isMoving = false;

            if (anim != null)
                anim.SetBool("andar", isMoving);
        }
        #endregion

        #region STATIC METHODS
        /// <summary>
        /// Retorna o CharacterMovementController contido neste GameObject, caso exista.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static CharacterMovementController GetController(GameObject go)
        {
            return go.GetComponent<CharacterMovementController>();
        }
        #endregion
    }
}