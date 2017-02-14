using System;
using UnityEngine;

namespace Assets.Scripts.Cenario.Room.Node {
    /// <summary>
    /// Esta classe representa um nodo situado em uma posição X e Z.
    /// Para se trabalhar com a altura de objetos é necessário adicionar
    /// a posição Y e explicitá-la nos pisos do cenário.
    /// </summary>
    [Serializable]
    public class Node
    {
        public NodeBehaviour Behaviour;

        /// <summary>
        /// Posições X e Y do nodo. Esta posição é única por nodo em uma sala.
        /// </summary>
        public int X;
        public int Z;

        /// <summary>
        /// Variável que representa se este nodo está ativo na grid, ou seja, se é possível que o personagem passe ou chegue até ele.
        /// </summary>
        public bool IsActive = true;
        public bool isAvailable = true;

        /// <summary>
        /// Inicializa o nodo definido suas posições X e Z e mantém uma referência para o seu respectivo NodeBehaviour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="behaviour"></param>
        public Node(int x, int z, NodeBehaviour behaviour)
        {
            this.X = x;
            this.Z = z;
            this.Behaviour = behaviour;
            IsActive = true;
            isAvailable = true;
        }

        /// <summary>
        /// Este método permite acesso facilitado a sala em que este nodo se encontra.
        /// </summary>
        /// <returns></returns>
        public RoomManager GetRoom()
        {
            return this.Behaviour.Room;
        }

        /// <summary>
        /// Retorna TRUE se as posições X e Z deste nodo são válidas.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !(this.X < 0 || this.Z < 0);
        }

        /// <summary>
        /// Retorna true caso este nodo esteja bloqueado por outro personagem.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            return isAvailable;
        }

        /// <summary>
        /// Retorna a posição deste nodo como um Vector3, onde sua posição
        /// Y não foi definida, ou seja, é igual a zero.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector3()
        {
            return new Vector3(this.X, 0f, this.Z);
        }

        /// <summary>
        /// Compara o nodo atual com outro. O retorno deste método é verdadeiro caso
        /// as posições X e Z de ambos os nodos sejam iguais.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Retorna TRUE caso os nodos sejam considerados iguais.</returns>
        public bool IsSameAs(Node other)
        {
            return(X == other.X && Z == other.Z);
        }
       
        /// <summary>
        /// Este método está depreciado! Use o operador "==" ao invés deste método.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Node)) return false;
            Node other = (Node) obj;
            return(this.IsSameAs(other));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Retorna uma string formatada contendo as posições X e Z.
        /// 
        /// O resultado deste método é a execução da seguinte linha de código abaixo:
        ///     "string.Format("[{0},{1}]", this.x, this.z)"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0},{1}]", this.X, this.Z);
        }
    }
}