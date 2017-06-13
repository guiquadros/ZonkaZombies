using System;
using ZonkaZombies.Util.Events;

namespace ZonkaZombies.Messaging
{
    public static class MessageRouter
    {
        public static void AddListener<T>(Action<T> callback)
        {
            GenericListActionListener<T>.AddListener(callback);
        }

        public static void RemoveListener<T>(Action<T> callback)
        {
            GenericListActionListener<T>.AddListener(callback);
        }

        public static void SendMessage<T>(T msg)
        {
            GenericListActionListener<T>.NotifyAll(msg);
        }
    }
}
