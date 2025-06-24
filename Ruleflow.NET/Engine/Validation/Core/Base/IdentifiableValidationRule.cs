using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    public abstract class IdentifiableValidationRule<T> : IValidationRule<T>
    {
        public string Id { get; }
        public int Priority { get; private set; }
        public ValidationSeverity Severity { get; private set; }

        protected IdentifiableValidationRule(string id)
        {
            Id = id;
            Severity = DefaultSeverity;
        }

        public void SetPriority(int priority) => Priority = priority;
        public void SetSeverity(ValidationSeverity severity) => Severity = severity;

        public abstract ValidationSeverity DefaultSeverity { get; }
        public abstract void Validate(T input);
    }
}
