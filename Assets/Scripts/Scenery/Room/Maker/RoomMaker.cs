using Assets.Scripts.Cenario.Interagivel;
using Assets.Scripts.Cenario.Room;
using Assets.Scripts.Cenario.Room.Node;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Scenery.Room.Maker
{
    public class RoomMaker : MonoBehaviour
    {
        #region EDITOR VARIABLES
        [Header("Configurações")]
        [SerializeField]
        private string nomeDaSala = string.Empty;
        [SerializeField]
        [Range(1, 3000)]
        private int altura = int.MinValue;
        [SerializeField]
        [Range(1, 3000)]
        private int largura = int.MinValue;
        [SerializeField]
        private GameObject floorPrefab;
        [SerializeField]
        private GameObject wallPrefab;
        #endregion

        #region STATIC VARIABLES
        private static readonly string ERROR_GO = "?Error?";
        #endregion

        #region PUBLIC VARIABLES
        public bool IsCreated { get { return this.isCreated; } }
        #endregion

        #region PRIVATE VARIABLES
        private float space = .27f;
        private bool isCreated;
        #endregion

        #region ROOMMAKER METHODS
        /// <summary>
        /// Destroi a sala existente, caso o cenário esteja sendo recriado.
        /// </summary>
        public void DestroyExistingRoom()
        {
            int childCount = transform.childCount;

            if (childCount != 0)
            {
                int index = 0;

                for (int i = 0; i < childCount; i++)
                {
                    if (transform.GetChild(index).GetComponent<NodeBehaviour>() != null)
                    {
                        DestroyImmediate(transform.GetChild(index).gameObject);
                    }
                    else
                    {
                        index++;
                    }
                }
            }

            isCreated = false;

            Transform wallsParentTransform = gameObject.transform.Find(string.Format("{0} - walls", name));

            if (wallsParentTransform != null)
                DestroyImmediate(wallsParentTransform.gameObject);
        }

        /// <summary>
        /// Método acessado pelo inspector, responsável por gerar um nodo cômodo com base nos 
        /// dados previamente estabelecidos pelo devenvolvedor, através do inspector.
        /// </summary>
        public void GenerateRoom()
        {
            //Realiza uma cópia de backup dos dados contidos no RoomManager atual, caso exista
            DestroyExistingRoom();
            CreateRoom();
            AddColider();

            RoomGridUpdater.EditorSetUpAllBoards(false);
        }

        ///Adiciona um novo BoxCollider e determina sua posição e tamanho, e um RigidBody.
        public void AddColider()
        {
            //Calcular posições X e Y com base na quantidade de nodos contidos no cômodo
            BoxCollider boxColliderScript = gameObject.GetComponent<BoxCollider>();

            if (!boxColliderScript)
            {
                boxColliderScript = gameObject.AddComponent<BoxCollider>();
            }

            //Atualizar posição do colisor ((X && Z) = (X || Z)*.5f - .5f)
            boxColliderScript.center =
                new Vector3(altura * .5f - .5f,
                    .6f, //Altura padrão para o colisor se manter dentro da sala
                    largura * .5f - .5f);

            //Atualizar o tamanho do colisor em (X || Z) -= .2f
            boxColliderScript.size =
                new Vector3(altura - .2f,
                1,
                largura - .2f);

            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();

            if (rigidBody == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }

            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
        }

        /// <summary>
        /// Método acessado pelo inspector que permite criar as paredes do cenário.
        /// </summary>
        public void GenerateWalls()
        {
            Transform wallsParentTransform = gameObject.transform.Find(string.Format("{0} - walls", name));

            if (wallsParentTransform != null)
                DestroyImmediate(wallsParentTransform.gameObject);

            //Creates a new wallsParent GameObject
            GameObject wallsParent = new GameObject(string.Format("{0} - walls", name));
            wallsParent.transform.SetParent(gameObject.transform);
            wallsParent.transform.position = Vector3.zero;
            wallsParent.transform.localPosition = Vector3.zero;

            for (int z = 0; z < largura; z++)
            {
                GameObject newWall = Instantiate(wallPrefab);
                newWall.AddComponent<VisibilityHandler>();
                newWall.transform.SetParent(wallsParent.transform);
                newWall.transform.localPosition = new Vector3(-.5f, 0, z);
                newWall.name = string.Format("wall [0,{0}]", z);
            }

            for (int x = 0; x < altura; x++)
            {
                GameObject newWall = Instantiate(wallPrefab);
                newWall.AddComponent<VisibilityHandler>();
                newWall.transform.SetParent(wallsParent.transform);
                newWall.transform.localPosition = new Vector3(x, 0, -.5f);
                newWall.transform.localEulerAngles = newWall.transform.localEulerAngles + new Vector3(0, 90, 0);
                newWall.name = string.Format("wall [{0},0]", x);
            }
        }

        /// <summary>
        /// Cria a disposição de nodos informada.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateRoom()
        {
            Debug.Log("RoomMaker [INFO]: Criando novo cenário...");

            if (nomeDaSala == string.Empty)
            {
                Debug.Log(
                    "RoomMaker [ERRO]: Não foi possível criar o cenário pois o nome do cômodo não foi informado!");
                gameObject.name = ERROR_GO;
                return;
            }
            else if (!floorPrefab)
            {
                Debug.Log(
                    "RoomMaker [ERRO]: Não foi possível criar o cenário pois o prefab a ser utilizado como piso não foi informado!");
                gameObject.name = ERROR_GO;
                return;
            }

            //Define o nome deste cômodo (GameObject)
            gameObject.name = nomeDaSala;

            RoomManager rm = gameObject.GetComponent<RoomManager>();

            if (rm == null)
                gameObject.AddComponent<RoomManager>();

            for (int x = 0; x < altura; x++)
            {
                for (int z = 0; z < largura; z++)
                {
                    //Instancia um objeto
                    GameObject newChild = Instantiate(floorPrefab);

                    newChild.transform.SetParent(transform);

                    //Define a posição local do nodo
                    newChild.transform.localPosition = new Vector3(x, 0, z);

                    //Adiciona o componente NodeBehavior
                    newChild.AddComponent<NodeBehaviour>();
                    newChild.AddComponent<VisibilityHandler>();

                    AbstractInteractor interactor = newChild.AddComponent<AbstractInteractor>();
                    interactor.SetType(InteractorTypeEnum.FLOOR);

                    //Atualiza o nome do nodo com base em suas posições locais X e Z
                    newChild.name = string.Format("[{0},{1}]", x, z);
                }
            }

            Debug.Log("RoomMaker [INFO]: O cenário foi criado com sucesso!");

            isCreated = true;
        }
        #endregion

        /// <summary>
        /// Script do editor que atualiza o "BoardManager" e seus filhos "NodeBehaviour"
        /// automaticamente.
        /// </summary>
        public class RoomGridUpdater
        {
            public static void EditorSetUpAllBoards(bool debug = false)
            {
                //Retorna o GameObject ativo na hierarquia
                //GameObject go = Selection.activeGameObject;

                //Recupera todos os RoomManagers da cena
                RoomManager[] roomManagers = FindObjectsOfType<RoomManager>();

                //O jogador deve ter selecionado um objeto na hierarquia
                if (roomManagers == null || roomManagers.Length == 0)
                {
                    if (debug)
                        Debug.Log("ERRO: Deve haver ao menos um objeto contendo o script RoomManager, na cena!");
                    return;
                }

                //Repetição

                int uniqueId = 0;

                for (int c = 0; c < roomManagers.Length; c++)
                {
                    if (roomManagers[c] == null)
                        continue;

                    EditorSetUpBoard(roomManagers[c], uniqueId);
                    uniqueId++;
                }
            }

            /// <summary>
            /// Configura o cômodo, adicionando seus nodos, nome e atualizando a referência RoomManager.
            /// </summary>
            /// <param name="room"></param>
            /// <param name="debug"></param>
            private static void EditorSetUpBoard(RoomManager room, int uniqueId, bool debug = false)
            {
                if (debug)
                    Debug.Log(string.Format(
                    "Buscando nodos da sala {0}..."
                    , room.gameObject.name));

                NodeBehaviour[] nodos =
                    room.gameObject.GetComponentsInChildren<NodeBehaviour>();

                /*Deve existir ao menos 1 componente do tipo "NodoBehaviour" 
                  filho do objeto "go" */
                if (nodos == null || nodos.Length == 0)
                {
                    if (debug)
                        Debug.Log("ERRO: Não foram encontrados componentes " +
                        "do tipo \"NodeBehaviour\" filhos do objeto \"" +
                        room.gameObject.name + "\".");
                    return;
                }

                if (debug)
                    Debug.Log(
                    string.Format("INFO: A sala {0} possui {1} nodos!",
                    room.gameObject.name, nodos.Length));

                int maxX = 0, maxZ = 0;

                //Para cada nodo, atualiza suas informações internas e seu nome
                for (int x = 0; x < nodos.Length; x++)
                {
                    nodos[x].UpdatePositionAndName(room);

                    if (maxX < nodos[x].GetNode().X)
                        maxX = nodos[x].GetNode().X;

                    if (maxZ < nodos[x].GetNode().Z)
                        maxZ = nodos[x].GetNode().Z;
                }

                room.Initialize(maxX + 1, maxZ + 1, uniqueId);

                if (debug)
                    Debug.Log(string.Format(
                    "A sala {0} teve seus nodos atualizados com sucesso!"
                    , room.gameObject.name));
            }
        }
    }
}