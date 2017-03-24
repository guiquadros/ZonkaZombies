using System.Collections.Generic;

// ReSharper disable InconsistentNaming
// ReSharper disable VirtualMemberCallInConstructor

namespace ZonkaZombies.Util
{
    /// <summary>
    /// This class is a generic holder that can save generic data in order to use it later. It also gives you a simplified way of reading and writing data.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class Memento<K>
    {
        private readonly Dictionary<K, object> SavedState = new Dictionary<K, object>();

        public void CreateState<V>(K key, V defaultState = default(V))
        {
            SavedState.Add(key, defaultState);
        }

        public V GetState<V>(K key)
        {
            return (V) SavedState[key];
        }

        public void SetState(K key, object value)
        {
            SavedState[key] = value;
        }
    }
}