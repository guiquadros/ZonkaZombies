using System.Collections;
using Assets.Scripts.Cenario.Room;
using Assets.Scripts.Cenario.Room.Node;
using Assets.Scripts.Cenario.Room.Node.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class CharacterMovementHandler : MonoBehaviour
    {
        #region DELEGATES
        public delegate void DestinationChanged(Node node);
        #endregion

        #region EVENTS
        public event DestinationChanged OnDestinationChanged;
        #endregion

        #region EVENTS DISPATCH
        /// <summary>
        /// Avisa aos observadores que este personagem trocou de posição.
        /// </summary>
        /// <param name="node"></param>
        private void DispatchOnDestinationChanged(Node node)
        {
            if (OnDestinationChanged != null)
                OnDestinationChanged(node);
        }
        #endregion

        #region EDITOR VARIABLES
        [Header("Ferramentas")]
        [SerializeField]
        private bool debug = false;
        #endregion

        #region PRIVATE VARIABLES
        private AStartNodePathfinding nodePathfinding;
        private CharacterMovementController controller;
        #endregion

        public void Initialize (CharacterMovementController controller)
        {
            this.controller = controller;
            nodePathfinding = new AStartNodePathfinding(controller.GetCurrentNode().GetRoom());
        }

        #region PRIVATE METHODS
        /// <summary>
        /// Cancela o movimento e animação do personagem.
        /// </summary>
        private void CancelCharacterMovement()
        {
            controller.MoveTo(null);
        }

        /// <summary>
        /// Método auxiliar que altera a sala atual do personagem.
        /// </summary>
        /// <param name="room"></param>
        private void ChangeActualRoom(RoomManager room)
        {
            nodePathfinding.SetRoom(room);
        }

        /// <summary>
        /// Busca o melhor caminho possível até o Nodo destino através de uma coroutina
        /// e inicia o movimento do personagem.
        /// </summary>
        /// <param name="newDestination"></param>
        public void MoveTo(Node newDestination)
        {
            if (!newDestination.GetRoom().IsSameAs(controller.GetCurrentNode().GetRoom()))
                return;
            if (newDestination.IsSameAs(controller.GetCurrentNode()))
                return;

            StopAllCoroutines();
            StartCoroutine(FindAndFollowPathToNode(newDestination));
        }

        /// <summary>
        /// Retorna o RoomManager atual do personagem.
        /// </summary>
        /// <returns></returns>
        private RoomManager GetCurrentRoom()
        {
            return controller.GetCurrentNode().Behaviour.Room;
        }

        /// <summary>
        /// Retorna o Nodo atual do personagem.
        /// </summary>
        /// <returns></returns>
        private Node GetCurrentNode()
        {
            return controller.GetCurrentNode();
        }
        #endregion

        #region COROUTINES
        /// <summary>
        /// Corotina responsável por realizar a busca de caminho 
        /// </summary>
        /// <param name="targetNode"></param>
        /// <returns></returns>
        private IEnumerator FindAndFollowPathToNode(Node targetNode)
        {
            yield return 0; //Aguardar 1 frame antes de iniciar a execução deste método

            //Executa o algoritmo A* através de uma Coroutine para buscar o melhor caminho até o nodo selecionado
            yield return nodePathfinding.GeneratePath(GetCurrentNode(), targetNode);

            yield return 0; //Aguarda um pouco antes de recuperar o caminho e revertê-lo

            controller.MoveTo(nodePathfinding.GetGeneratedPath());
        }
        #endregion
    }

}