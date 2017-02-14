using UnityEngine;

namespace Assets.Scripts.Cenario.Room.Node
{
    /// <summary>
    /// Esta classe é utilizada pelo algoritmo A* a fim de se encontrar o melhor
    /// caminho possível entre a posição atual e a posição destino. Esta classe
    /// encapsula os calculos de algoritmo.
    /// </summary>
    public class AStarNode
    {
        public Node node;

        private AStarNode parent;
        public AStarNode Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        private int g, h;
        public int G { get { return this.g; } }
        public int H { get { return this.h; } }
        public int F { get { return (g + h); } }

        public AStarNode(Node node)
        {
            this.node = node;
            Reset();
        }

        /// <summary>
        /// Retorna TRUE .
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSameAs(AStarNode other)
        {
            return other.node.IsSameAs(node);
        }

        /// <summary>
        /// Calcula o valor de G deste nodo. É necessário definir seu parent para poder 
        /// realizar esta ação.
        /// 
        /// Movimentos diagonais = 14
        /// Movimentos ortogonais = 10
        /// </summary>
        public void updateG()
        {
            g = parent.G + (parent.node.X != node.X && parent.node.Z != node.Z ? 14 : 10);
        }

        /// <summary>
        /// Calcula o valor de H com base no destino buscado pelo algoritmo.
        /// </summary>
        /// <param name="end"></param>
        public void updateH(Node end)
        {
            // Método de Manhattan
            int difX = Mathf.FloorToInt(Mathf.Abs(node.X - end.X));
            int difY = Mathf.FloorToInt(Mathf.Abs(node.Z - end.Z));
            h = (difX + difY) * 10;
        }

        /// <summary>
        /// Reseta os valores de G e H e o parent do nodo.
        /// </summary>
        public void Reset()
        {
            parent = null;
            g = 0;
            h = 0;
        }

        /// <summary>
        /// Este método está depreciado! Use o operador "==" ao invés deste método.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(AStarNode)) return false;
            AStarNode other = (AStarNode)obj;
            return (node.IsSameAs(other.node));
        }

        public override int GetHashCode()
        {
            return node.GetHashCode();
        }

        /*
public int CompareTo(AStarNode nodeToCompare)
{
    int compare = F.CompareTo(nodeToCompare.F);
    if (compare == 0)
        compare = H.CompareTo(nodeToCompare.H);
    return -compare;
}
*/

    }

}
