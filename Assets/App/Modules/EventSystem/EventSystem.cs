using System.Collections.Generic;
using System;

namespace App.Events
{
    public static class EventSystem
    {
        private static Dictionary<Type, object> _events;

        static EventSystem()
        {
            _events = new Dictionary<Type, object>();
        }

        public static void Subscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);

            if (!_events.ContainsKey(eventType))
            {
                _events.Add(eventType, new ActionWrapper<T>());
            }

            ((ActionWrapper<T>)_events[eventType]).Action += handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);

            if (!_events.ContainsKey(eventType))
            {
                return;
            }

            ((ActionWrapper<T>)_events[eventType]).Action -= handler;
        }

        public static void Send<T>(T eventData)
        {
            var eventType = typeof(T);

            if (!_events.ContainsKey(eventType))
            {
                return;
            }

            ((ActionWrapper<T>)_events[eventType]).Invoke(eventData);
        }

        public static void Send<T>() where T : new()
        {
            var eventType = typeof(T);

            if (!_events.ContainsKey(eventType))
            {
                return;
            }

            ((ActionWrapper<T>)_events[eventType]).Invoke(new T());
        }
    }

    internal sealed class ActionWrapper<T>
    {
        public event Action<T> Action;

        public void Invoke(T data)
        {
            Action?.Invoke(data);
        }
    }
}