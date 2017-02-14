using UnityEngine;

namespace Assets.Scripts.Cenario.Room.Door
{
    [DisallowMultipleComponent]
    public class DoorBehaviour : MonoBehaviour
    {
        #region EDITOR VARIABLES
        [SerializeField]
        private DoorStateEnum state;
        [SerializeField]
        private DoorTypeEnum type;
        [SerializeField]
        private RoomManager[] roomsConnected;
        #endregion

        #region PUBLIC VARIABLES
        public DoorStateEnum State { get { return state; } }
        public DoorTypeEnum Type { get { return type; } }
        #endregion

        #region PRIVATE VARIABLES
        /// <summary>
        /// Mantém o transform deste objeto, para acesso otimizado.
        /// </summary>
        [System.NonSerialized]
        public new Transform transform;
        #endregion

        private void Awake()
        {
            this.transform = GetComponent<Transform>();
        }

        private void Start()
        {
            if (roomsConnected != null && roomsConnected.Length >= 2)
                throw new System.NullReferenceException("ERRO: roomsConnected não pode possuir mais do que duas conexões!");
        }

        /// <summary>
        /// Retorna a referência para o outro RoomManager conectado ao RoomManager atual.
        /// </summary>
        /// <param name="actualRoom"></param>
        /// <returns></returns>
        public RoomManager GetOtherRoom(RoomManager actualRoom)
        {
            if (roomsConnected[0].IsSameAs(actualRoom))
                return roomsConnected[0];
            return roomsConnected[1];
        }

        /// <summary>
        /// Retorna o componente DoorBehaviour contido no GameObject, caso exista.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static DoorBehaviour GetBehaviour(GameObject go)
        {
            return go.GetComponent<DoorBehaviour>();
        }

    }

}