using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ruleflow.NET.Engine.Events
{
    /// <summary>
    /// Simple in-memory event hub for registering and triggering events.
    /// </summary>
    public static class EventHub
    {
        private static readonly Dictionary<string, List<Action>> _handlers = new();
        public class EventHubLog {}
        public static ILogger<EventHubLog> Logger { get; private set; } = NullLogger<EventHubLog>.Instance;

        public static void SetLogger(ILogger<EventHubLog>? logger)
        {
            Logger = logger ?? NullLogger<EventHubLog>.Instance;
        }

        /// <summary>
        /// Registers a handler for the specified event.
        /// </summary>
        public static void Register(string eventName, Action handler)
        {
            Logger.LogInformation("Registering handler for event {Event}", eventName);
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
            Logger.LogInformation("Triggering event {Event}", eventName);
            if (_handlers.TryGetValue(eventName, out var list))
            {
                foreach (var h in list)
                {
                    try
                    {
                        h();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Event handler for {Event} failed", eventName);
                    }
                }
            }
        }

        /// <summary>
        /// Clears all registered events and handlers.
        /// </summary>
        public static void Clear()
        {
            Logger.LogInformation("Clearing all event handlers");
            _handlers.Clear();
        }
    }
}
