using System;
using System.Collections.Generic;

namespace ZonkaZombies.Util.Events
{
    public sealed class ListActionListener
    {
        private readonly List<Action> _listeners = new List<Action>();

        public void AddListener(Action listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(Action listener)
        {
            _listeners.Remove(listener);
        }

        public void NotifyAll()
        {
            _listeners.ForEach(o => o.Invoke());
        }
    }
}