using System;
using System.Collections.Generic;

namespace ZonkaZombies.Util.Events
{
    /// <summary>
    /// WARNING: YOU CANNOT UNLOAD ALL SCENES OF THE GAME. Otherwise you will lost the static references of the message classes. So keep the Persistent scene always loaded in the game.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class GenericListActionListener<T>
    {
        private static readonly List<Action<T>> Listeners = new List<Action<T>>();

        public static void AddListener(Action<T> listener)
        {
            Listeners.Add(listener);
        }

        public static void RemoveListener(Action<T> listener)
        {
            Listeners.Remove(listener);
        }

        public static void NotifyAll(T message)
        {
            Listeners.ForEach(o => o.Invoke(message));
        }
    }

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