using System;

namespace Assets.Scripts.Cenario.Room.Node.Pathfinding.Generic
{
    /// <summary>
    /// Esta classe representa uma estrutura de árvore binária e serve para
    /// mantermos itens estocados que serão utilizados mais tarde. Desta forma,
    /// esta classe mantém os itens organizados por prioridades, e sempre que for
    /// necessário recuperar o item menor (ou no caso do A*, o F de menor custo)
    /// esta classe nos recupera o item instantaneamente.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Heap<T> where T : IHeapItem<T>
    {
        #region PRIVATE VARIABLES
        private T[] items;
        private int currentItemCount;
        #endregion

        #region HEAP METHODS
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        /// <summary>
        /// Adiciona novos itens no Heap.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        /// <summary>
        /// Retorna a quantidade de elementos neste Heap.
        /// </summary>
        public int Count
        {
            get
            {
                return currentItemCount;
            }
        }

        /// <summary>
        /// Retorna TRUE se este item existe no array.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Compara o valor do novo item com base no valor de seus parents 
        /// para mantê-lo em uma posição adequada no array, de acordo com 
        /// sua prioridade.
        /// </summary>
        /// <param name="item"></param>
        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Altera a posição dos itens.
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
    #endregion

    /// <summary>
    /// Interface que define um item como sendo um HeapItem, tornando possível manipulá-lo através da classe Heap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }

}