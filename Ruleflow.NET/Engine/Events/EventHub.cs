using System;
using System.Collections.Generic;

namespace Ruleflow.NET.Engine.Events
{
    /// <summary>
    /// Simple in-memory event hub for registering and triggering events.
    /// </summary>
    public static class EventHub
    {
        private static readonly Dictionary<string, List<Action>> _handlers = new();

        /// <summary>
        /// Registers a handler for the specified event.
        /// </summary>
        public static void Register(string eventName, Action handler)
        {
            if (!_handlers.TryGetValue(eventName, out var list))
            {
                list = new List<Action>();
                _handlers[eventName] = list;
            }
            list.Add(handler);
        }

        /// <summary>
        /// Triggers an event and invokes all registered handlers.
        /// </summary>
        public static void Trigger(string eventName)
        {
            if (_handlers.TryGetValue(eventName, out var list))
            {
                foreach (var h in list)
                    h();
            }
        }

        /// <summary>
        /// Clears all registered events and handlers.
        /// </summary>
        public static void Clear()
        {
            _handlers.Clear();
        }
    }
}
