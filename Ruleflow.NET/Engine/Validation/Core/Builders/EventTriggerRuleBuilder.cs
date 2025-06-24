using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    /// <summary>
    /// Builder pro pravidlo vyvolávající událost.
    /// <para>Builder for a rule that triggers an event.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
    public class EventTriggerRuleBuilder<T>
    {
        private string _id = Guid.NewGuid().ToString();
        private string? _eventName;
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Information;

        /// <summary>
        /// Nastaví identifikátor pravidla.
        /// <para>Sets the rule identifier.</para>
        /// </summary>
        public EventTriggerRuleBuilder<T> WithId(string id) { _id = id; return this; }

        /// <summary>
        /// Určí název události, která má být vyvolána.
        /// <para>Specifies the event name to trigger.</para>
        /// </summary>
        public EventTriggerRuleBuilder<T> WithEvent(string eventName) { _eventName = eventName; return this; }

        /// <summary>
        /// Nastaví prioritu pravidla.
        /// <para>Sets the rule priority.</para>
        /// </summary>
        public EventTriggerRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }

        /// <summary>
        /// Nastaví závažnost.
        /// <para>Sets the severity level.</para>
        /// </summary>
        public EventTriggerRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

        /// <summary>
        /// Vytvoří pravidlo, které spustí požadovanou událost.
        /// <para>Builds the rule that triggers the configured event.</para>
        /// </summary>
        public EventTriggerValidationRule<T> Build()
        {
            if (string.IsNullOrEmpty(_eventName))
                throw new InvalidOperationException("Event name not set");
            var rule = new EventTriggerValidationRule<T>(_id, _eventName);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
