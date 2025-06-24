using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    public class EventTriggerRuleBuilder<T>
    {
        private string _id = Guid.NewGuid().ToString();
        private string? _eventName;
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Information;

        public EventTriggerRuleBuilder<T> WithId(string id) { _id = id; return this; }
        public EventTriggerRuleBuilder<T> WithEvent(string eventName) { _eventName = eventName; return this; }
        public EventTriggerRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }
        public EventTriggerRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

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
