using System.Collections.Generic;
using Assets.Scripts.Cenario.Room.Node;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Cenario.Room
{
    [DisallowMultipleComponent]
    public class RoomManager : MonoBehaviour
    {
        #region EDITOR VARIABLES
        [Header("!NÃO ALTERE!")]
        //Estes campos precisam ser serializaveis para que seja possível executar
        //o script no Editor.
        [SerializeField]
        private int ID;
        [SerializeField]
        private int boardSizeX;
        public int BoardSizeX { get { return boardSizeX; } }
        [SerializeField]
        private int boardSizeZ;
        public int BoardSizeZ { get { return boardSizeZ; } }

        private Node.Node[,] board;
        public Node.Node[,] Board { get { return board; } }

        [Header("Configurações")]
        [SerializeField]
        private RoomConnection[] roomConnections;
        public RoomConnection[] RoomConnections { get { return roomConnections; } }

        [Header("Hack - Visibility Test")]
        [SerializeField]
        private bool enable;
        [SerializeField]
        private bool disable;
        #endregion

        #region PUBLIC VARIABLES
        public int neighborsCount { get { return roomConnections == null ? 0 : roomConnections.Length; } }
        public int MaxSize { get { return boardSizeX * boardSizeZ; } }
        #endregion

        #region PRIVATE VARIABLES
        /// <summary>
        /// Array fixo que mantém apenas os objetos estáticos do cenário, como a mobilia.
        /// </summary>
        private IChangeVisibility[] staticHandlers;
        /// <summary>
        /// Lista dinâmica que mantém os NPCs que acessam o cenário, alterando sua visibilidade em conjunto 
        /// com os objetos estáticos.
        /// </summary>
        private List<IChangeVisibility> dynamicHandlers = new List<IChangeVisibility>();
        #endregion

        #region MONOBEHAVIOUR METHODS
        /// <summary>
        /// Inicializa o cômodo buscando todos os nodos contidos no cenário e
        /// os adicionando em um array.
        /// </summary>
        private void Awake()
        {
            staticHandlers = GetComponentsInChildren<IChangeVisibility>();
            CreateRoom();
        }

        private void Start()
        {
            TurnRoomInvisible();
        }

        /// <summary>
        /// Restaura o valor da variável roomConnections, contidos neste RoomManager. Este método
        /// é util para as classes que precisam manipular este componente através do inspetor.
        /// </summary>
        /// <param name="roomConnections"></param>
        public void Restore(RoomConnection[] roomConnections)
        {
            this.roomConnections = roomConnections;
        }

        private void Update()
        {
            if (enable)
            {
                enable = false;
                TurnRoomVisible();
            }
            else if (disable)
            {
                disable = false;
                TurnRoomInvisible();
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Torna tudo o que estiver dentro do cômodo, visível.
        /// </summary>
        private void TurnRoomVisible()
        {
            for (int i = 0; i < staticHandlers.Length; i++)
            {
                staticHandlers[i].TurnVisible();
            }

            for (int i = 0;i < dynamicHandlers.Count; i++)
            {
                dynamicHandlers[i].TurnVisible();
            }
        }

        /// <summary>
        /// Torna tudo o que estiver dentro do cômodo, invisível
        /// </summary>
        private void TurnRoomInvisible()
        {
            for (int i = 0; i < staticHandlers.Length; i++)
            {
                staticHandlers[i].TurnInvisible();
            }

            for (int i = 0; i < dynamicHandlers.Count; i++)
            {
                dynamicHandlers[i].TurnInvisible();
            }
        }
        #endregion

#if UNITY_EDITOR
        public void Initialize(int x, int z, int uniqueId)
        {
            boardSizeX = x;
            boardSizeZ = z;
            ID = uniqueId;

            CreateRoom();
        }

        /// <summary>
        /// Inicializa a matriz de nodeos desta sala.
        /// </summary>
        private void CreateRoom()
        {
            if (boardSizeX <= 0 || boardSizeZ <= 0)
                throw new System.ArgumentNullException();

            board = new Node.Node[boardSizeX, boardSizeZ];

            NodeBehaviour[] nodes = transform.GetComponentsInChildren<NodeBehaviour>();

            for (int c = 0; c < nodes.Length; c++)
            {
                if (!nodes[c].GetNode().IsValid())
                    continue;

                if (nodes[c].GetNode().X >= boardSizeX || nodes[c].GetNode().Z >= boardSizeZ)
                {
                    throw new System.NullReferenceException(string.Format("{0} [{1},{2}] não é válido!", 
                        name, nodes[c].GetNode().X, nodes[c].GetNode().Z));
                }

                board[nodes[c].GetNode().X, nodes[c].GetNode().Z] = nodes[c].GetNode();
            }
        }

        private void OnDrawGizmos()
        {
            if (boardSizeZ != 0 && boardSizeX != 0)
            {
                //Desenha o cubo em volta do cômodo
                Gizmos.color = Color.black;
                float altura = 3;
                Gizmos.DrawWireCube(gameObject.transform.position +
                    new Vector3((boardSizeX - 1) * .5f, altura * .5f, (boardSizeZ - 1) * .5f),
                    Vector3.right * boardSizeX + Vector3.up * altura + Vector3.forward * boardSizeZ);
            }

            if (roomConnections != null)
            {
                for (int i = 0; i < roomConnections.Length; i++)
                {
                    if (roomConnections[i].ThisRoomNode == null)
                        continue;

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(roomConnections[i].ThisRoomNode.transform.position, new Vector3(.8f, .2f, .8f));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (boardSizeZ != 0 && boardSizeX != 0)
            {
                //Desenha o cubo em volta do cômodo
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(gameObject.transform.position +
                    new Vector3((boardSizeX - 1) * .5f, .99f, (boardSizeZ - 1) * .5f),
                    Vector3.right * boardSizeX * .98f + Vector3.up * 2 + Vector3.forward * boardSizeZ * .98f);
            }

            if (roomConnections != null)
            {
                for (int i = 0; i < roomConnections.Length; i++)
                {
                    if (roomConnections[i].OtherRoomNode == null)
                        continue;

                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(roomConnections[i].OtherRoomNode.transform.position, new Vector3(.4f, .5f, .4f));
                }
            }
        }
#endif

        /// <summary>
        /// Deprecated.
        /// Remover este método assim que possível.
        /// </summary>
        /// <param name="connectedRoom"></param>
        /// <returns></returns>
        public Node.Node GetNeighbourRoomNode(RoomManager connectedRoom)
        {
            for (int c = 0; c < roomConnections.Length; c++)
            {
                if (roomConnections[c].OtherRoomNode.Room.IsSameAs(connectedRoom))
                    return roomConnections[c].OtherRoomNode.GetNode();
            }

            //Não há conexão entre as salas
            return null;
        }

        /// <summary>
        /// Retorna um Array contendo os nodos vizinhos ao nodo selecionado. 
        /// Caso não haja algum vizinho do nodo atual, ele é retornado com o 
        /// valor nulo, no Array. Ao encontrar o primeiro valor nulo no Array 
        /// é possível dar um break na estrutura de repetição, pois os valores 
        /// nulos sempre serão os últimos do Array.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Array contendo os vizinhos do nodo selecionado.</returns>
        public Node.Node[] GetNeighborsOf(Node.Node node)
        {
            Node.Node[] neighbors = new Node.Node[8];
            int index = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    if (x == 0 && z == 0) //Pula a casa em que o jogador está
                    {
                        continue;
                    }

                    int correctX = (x + node.X);
                    int correctZ = (z + node.Z);

                    if (correctX < 0 || correctZ < 0)
                    {
                        continue;
                    }

                    if (correctX >= this.boardSizeX ||
                        correctZ >= this.boardSizeZ)
                    {
                        continue;
                    }

                    if (board[correctX, correctZ] == null ||
                        !board[correctX, correctZ].IsActive)
                    {
                        continue;
                    }

                    neighbors[index] = board[correctX, correctZ];
                    index++;
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Retorna TRUE caso ambos os objetos apontem para a mesma sala.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSameAs(RoomManager other)
        {
            return (this.ID == other.ID);
        }

        private int npchInRoom = 0;
        private bool isVisible = false;

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("NPCH"))
            {
                npchInRoom++;
                if (!isVisible && npchInRoom > 0)
                {
                    isVisible = true;
                    TurnRoomVisible();
                }
            }
            else if (collider.gameObject.CompareTag("NPC"))
            {
                //Habilita ou desabilita o NPC conforme necessário
                IChangeVisibility npcChangeVisibility = collider.gameObject.GetComponent<IChangeVisibility>();

                if (npcChangeVisibility == null)
                {
                    Debug.Log(string.Format("ERRO: O inimigo {0} não possui um componente do tipo IChangeVisibility!",
                        collider.gameObject.name));
                    return;
                }

                dynamicHandlers.Add(npcChangeVisibility);

                if (isVisible)
                    npcChangeVisibility.TurnVisible();
                else
                    npcChangeVisibility.TurnInvisible();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("NPCH"))
            {
                npchInRoom--;
                if (isVisible && npchInRoom == 0)
                {
                    isVisible = false;
                    TurnRoomInvisible();
                }
            } else if (collider.gameObject.CompareTag("NPC"))
            {
                //Habilita ou desabilita o NPC conforme necessário
                IChangeVisibility npcChangeVisibility = collider.gameObject.GetComponent<IChangeVisibility>();

                if (npcChangeVisibility == null)
                {
                    Debug.Log(string.Format("ERRO: O inimigo {0} não possui um componente do tipo IChangeVisibility!", 
                        collider.gameObject.name));
                    return;
                }

                if (dynamicHandlers.Contains(npcChangeVisibility))
                    dynamicHandlers.Remove(npcChangeVisibility);
            }
        }

    }
}