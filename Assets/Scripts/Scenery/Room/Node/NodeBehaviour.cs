using UnityEngine;

namespace Assets.Scripts.Cenario.Room.Node
{
    /// <summary>
    /// Esta classe deve ser adicionada nos pisos do cenário para que o Raycast
    /// o recupere no momento em que o piso for selecionado. Desta forma, pretende-se
    /// realizar as ações de movimento e interação com os objetos do cenário.
    /// </summary>
    [DisallowMultipleComponent]
    public class NodeBehaviour : MonoBehaviour
    {
        #region EDITOR VARIABLES
        [SerializeField]
        private Node node;

        [SerializeField]
        private float gizmoOffset = .0f;

        [Header("!NÃO ALTERE!")]
        [SerializeField]
        private RoomManager room;
        public RoomManager Room { get { return room; } }
        #endregion

        #region MONOBEHAVIOUR METHODS
        private void Awake()
        {
            node.Behaviour = this;
        }
        #endregion

        #region NODEBEHAVIOUR METHODS
        public Node GetNode()
        {
            return this.node;
        }

        /// <summary>
        /// Retorna o NodeBahavior contido no GameObject, caso exista.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static NodeBehaviour GetBehaviour(GameObject go)
        {
            return go.GetComponent<NodeBehaviour>();
        }
        #endregion
        
        /// <summary>
        /// Atualiza as posições X e Z deste objeto, no Node interno da classe, com base
        /// nas posições definidas na janela do Editor (Inspector).
        /// </summary>
        public void UpdatePositionAndName(RoomManager myRoom)
        {
            this.room = myRoom;

            /*Define a posição deste nodo, automaticamente, buscando a posição X e Z 
                          do mesmo, na cena */
            node = new Node(
                Mathf.FloorToInt(transform.localPosition.x),
                Mathf.FloorToInt(transform.localPosition.z),
                this);

            //Altera o nome para ficar mais compreensível no Editor [x,z]
            this.gameObject.name = 
                string.Format("{0} {1}", myRoom.gameObject.name ,node.ToString());
        }

        private void OnDrawGizmos()
        {
            if (this.node == null || !this.node.IsValid())
                return; //Se a posição não for válida, não apresente o Gizmo

            Gizmos.color = this.node.IsActive ? Color.blue : Color.red;
            Vector3 desiredGizmosPos = transform.position + 
                new Vector3(0, gizmoOffset, 0);

            Gizmos.DrawWireCube(desiredGizmosPos,
                    new Vector3(1, 0, 1));

            if (!this.node.IsActive)
            {
                Gizmos.DrawWireCube(desiredGizmosPos,
                    new Vector3(.7f, 0, .7f));
                Gizmos.DrawCube(desiredGizmosPos,
                    new Vector3(.4f, 0, .4f));
            } else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(desiredGizmosPos, Vector3.one * .1f);
            }

            if (!this.node.IsActive)
                return;
        }
    }
}