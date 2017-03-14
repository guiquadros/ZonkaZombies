using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable VirtualMemberCallInConstructor

namespace TinyCrew.Util
{
    public abstract class Memento<K>
    {
        private readonly Dictionary<K, object> SavedState = new Dictionary<K, object>();

        /// <summary>
        /// Save the state of this object to be read later. Call this method AFTER reading the input.
        /// </summary>
        public void SaveState()
        {
            OnSaveState();
        }

        /// <summary>
        /// Use this method to initialize all the values needed that are going to be saved and loaded later.
        /// </summary>
        internal virtual void OnCreateStates() { }

        /// <summary>
        /// Use this method to persist the current state of the object, calling the method "SaveState".
        /// </summary>
        protected virtual void OnSaveState() { }

        protected void CreateState<V>(K key, V defaultState = default(V))
        {
            SavedState.Add(key, defaultState);
        }

        protected V LoadState<V>(K key)
        {
            return (V) SavedState[key];
        }

        protected void SaveState(K key, object value)
        {
            SavedState[key] = value;
        }
    }
}