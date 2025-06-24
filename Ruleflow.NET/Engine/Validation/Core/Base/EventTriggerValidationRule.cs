using Ruleflow.NET.Engine.Events;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Validation rule that triggers a named event when executed.
    /// </summary>
    public class EventTriggerValidationRule<T> : IdentifiableValidationRule<T>
    {
        private readonly string _eventName;
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Information;

        public EventTriggerValidationRule(string id, string eventName) : base(id)
        {
            _eventName = eventName;
        }

        public override void Validate(T input)
        {
            EventHub.Trigger(_eventName);
        }
    }
}
