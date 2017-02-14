using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.Diagnostics;

namespace Assets.Scripts.Cenario.Room.Node.Pathfinding
{
    public class AStartNodePathfinding
    {
        #region PRIVATE VARIABLES
        /// <summary>
        /// BoardManager que representa o cenario.
        /// </summary>
        private RoomManager board;
        private List<Node> generatedPath;
        #endregion

        #region ASTARNODEPATHFINDING METHODS
        public AStartNodePathfinding(RoomManager board)
        {
            if (board == null)
                Debug.LogError("RoomManager can't be null!");

            this.board = board;
            //sw = new Stopwatch();
        }

        /// <summary>
        /// Altera o grid utilizado para realização dos cálculos. É necessário alterar a grid sempre que 
        /// houver a troca de salas.
        /// A grid só é alterada caso não seja a mesma existente nesta classe.
        /// </summary>
        /// <param name="room"></param>
        public void SetRoom(RoomManager room)
        {
            if (room.IsSameAs(board))
                return;
            board = room;
        }

        //Stopwatch sw;

        /**
         * A partir da execucao do algoritmo de busca de caminho chamado A* - ou A-Estrela -, o caminho mais curto entre os pontos
         * origem e destino (parametros) desconsiderando os nodos definidos como nao andavel e tambem aos Nodos contidos na lista 
         * enemies (parametros).
         */
        public IEnumerator GeneratePath(Node origem, Node destino)
        {
            List<AStarNode> aberta = new List<AStarNode>();
            HashSet<AStarNode> fechada = new HashSet<AStarNode>();

            //Transofmr os Node origem em um AStarNode, necessário 
            //para a realização dos cálculos
            AStarNode aStarOrigem = new AStarNode(origem);
            aStarOrigem.updateH(destino);

            aberta.Add(aStarOrigem);

            while (aberta.Count > 0)
            {
                AStarNode nodoAtual = GetMinF(aberta);

                // Se o nodo atual for o nodo destino que esta sendo buscado o caminho e reconstruido e retornado
                if (nodoAtual.node.IsSameAs(destino))
                {
                    // O caminho e reconstruido em uma lista onde o indice 0 e igual ao ponto de origem e o indice final e o ponto de destino
                    this.generatedPath = ReconstructPath(nodoAtual);

                    // Retorna o caminho pra o metodo que o chamou
                    yield break;
                }

                aberta.Remove(nodoAtual); //Remove da lista aberta
                fechada.Add(nodoAtual);   //Adiciona na lista fechada

                //Busca pelos vizinhos do nodo atual
                Node[] neighbors = board.GetNeighborsOf(nodoAtual.node);

                for (int c = 0; c < neighbors.Length; c++)
                {
                    if (neighbors[c] == null)
                        continue;

                    AStarNode starNode = ToAStarNode(neighbors[c], aberta);

                    //AStarNode starNode = ToAStarNode(neighbors[c]);

                    // Se o vizinho ja esta na lista fechada, ele nao 
                    //deve ser testado novamente
                    if (fechada.Contains(starNode))
                        continue;

                    //Se a lista aberta não contiver o nodo...
                    if (!aberta.Contains(starNode))
                    {
                        //Torna o nodo atual parten do nodo vizinho
                        starNode.Parent = nodoAtual;
                        //Atualizar o valor H
                        starNode.updateH(destino);
                        //Atualizar o valor G
                        starNode.updateG();
                        //Adicionar na lista aberta
                        aberta.Add(starNode);
                    }
                    else
                    {
                        if (nodoAtual.G < starNode.Parent.G)
                        {
                            starNode.Parent = nodoAtual;
                            starNode.updateG();
                        }
                    }
                }

                yield return 0;
            }
            // Nao foi encontrado um caminho possivel entre os pontos origem e destino
            this.generatedPath = null;
        }

        private AStarNode ToAStarNode(Node node, List<AStarNode> aberta)
        {
            for (int c = 0; c < aberta.Count; c++)
                if (aberta[c].node.IsSameAs(node))
                    return aberta[c];

            return new AStarNode(node);
        }

        /// <summary>
        /// Retorna uma lista contendo o melhor caminho, após a execução do método
        /// "GeneratePath()". Caso contrário, retorna NULL.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetGeneratedPath()
        {
            if (generatedPath != null)
                generatedPath.Reverse(); //Inverte a lista de Nodes

            return this.generatedPath;
        }

        /// <summary>
        /// Retorna o nodo da lista com o menor valor de F.
        /// 
        /// F = (G + H)
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private AStarNode GetMinF(List<AStarNode> lista)
        {
            AStarNode min = null;

            for (int c = 0; c < lista.Count; c++)
            {
                if (min == null)
                {
                    min = lista[c];
                    continue;
                }

                if (lista[c].F == min.F)
                {
                    if (lista[c].H < min.H)
                    {
                        min = lista[c];
                    }
                    else if (lista[c].G <= min.G)
                    {
                        min = lista[c];
                    }
                }
                else
                {
                    if ((lista[c].F < min.F))
                    {
                        min = lista[c];
                    }
                }

            }
            return min;
        }

        /// <summary>
        /// Reconstrói o caminho encontrado até o nodo final.
        /// </summary>
        /// <param name="final"></param>
        /// <returns></returns>
        private List<Node> ReconstructPath(AStarNode final)
        {
            List<Node> resultList = new List<Node>();
            while (final.Parent != null)
            {
                resultList.Add(final.node);
                final = final.Parent;
            }
            return resultList;
        }
        #endregion
    }
}
