using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Cenario.Room.Pathfinding.Node;

namespace Assets.Scripts.Cenario.Room.Pathfinding
{
    public class AStartRoomPathfinding
    {
        private List<RoomManager> generatedPath;

        /**
            Deprecated! (Este método não deve funcionar!
         * A partir da execucao do algoritmo de busca de caminho chamado A* - ou A-Estrela -, o caminho mais curto entre os pontos
         * origem e destino (parametros) desconsiderando os nodos definidos como nao andavel e tambem aos Nodos contidos na lista 
         * enemies (parametros).
         */
        public IEnumerator GeneratePath(RoomManager origem, RoomManager destino)
        {
            List<AStarRoom> aberta = new List<AStarRoom>();
            HashSet<AStarRoom> fechada = new HashSet<AStarRoom>();

            aberta.Add(new AStarRoom(origem));

            while (aberta.Count > 0)
            {
                AStarRoom actualRoom = GetBestRoom(aberta);

                aberta.Remove(actualRoom);
                fechada.Add(actualRoom);

                RoomManager[] neighbors = null; //actualRoom.room.Neighbours;

                for (int c = 0; c < neighbors.Length; c++)
                {
                    if (neighbors[c] == null)
                        continue;

                    AStarRoom starRoom = ToAStarRoom(neighbors[c], aberta);

                    // Se o vizinho ja esta na lista fechada, ele nao 
                    //deve ser testado novamente
                    if (fechada.Contains(starRoom))
                        continue;

                    //Se a lista aberta não contiver o nodo...
                    if (!aberta.Contains(starRoom))
                    {
                        //Torna o nodo atual parten do nodo vizinho
                        starRoom.parent = actualRoom;

                        // Se o nodo atual for o nodo destino que esta sendo buscado o caminho e reconstruido e retornado
                        if (starRoom.room.IsSameAs(destino))
                        {
                            // O caminho e reconstruido em uma lista onde o indice 0 e igual ao ponto de origem e o indice final e o ponto de destino
                            this.generatedPath = ReconstructPath(starRoom);

                            // Retorna o caminho pra o metodo que o chamou
                            yield break;
                        }

                        //Adicionar na lista aberta
                        aberta.Add(starRoom);
                    }
                }
            }
            // Nao foi encontrado um caminho possivel entre os pontos origem e destino
            this.generatedPath = null;
        }

        /// <summary>
        /// Retorna o AStarRoom vinculado ao RoomManager, contido na lista aberta.
        /// Caso o RoomManager não existe, um novo RoomManager é criado.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="aberta"></param>
        /// <returns></returns>
        private AStarRoom ToAStarRoom(RoomManager room, List<AStarRoom> aberta)
        {
            for (int c = 0; c < aberta.Count; c++)
                if (aberta[c].room.IsSameAs(room))
                    return aberta[c];
            return new AStarRoom(room);
        }

        /// <summary>
        /// Retorna uma lista contendo o melhor caminho, após a execução do método
        /// "GeneratePath()". Caso contrário, retorna NULL.
        /// </summary>
        /// <returns></returns>
        public List<RoomManager> GetGeneratedPath()
        {
            if (generatedPath != null)
                generatedPath.Reverse(); //Inverte a lista de Rooms
            return generatedPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private AStarRoom GetBestRoom(List<AStarRoom> lista)
        {
            AStarRoom max = lista[0];
            for (int c = 1; c < lista.Count; c++)
                if (lista[c].room.neighborsCount > max.room.neighborsCount)
                    max = lista[c];
            //Retorna o RoomManager com a maior quantidade de vizinhos
            return max;
        }

        /// <summary>
        /// Reconstrói o caminho encontrado até o nodo final.
        /// </summary>
        /// <param name="final"></param>
        /// <returns></returns>
        private List<RoomManager> ReconstructPath(AStarRoom final)
        {
            List<RoomManager> resultList = new List<RoomManager>();
            do {
                resultList.Add(final.room);
                final = final.parent;
            } while (final != null);
            return resultList;
        }
    }
}
