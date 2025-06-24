using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Interfaces
{
    public interface IValidationRule<T>
    {
        string Id { get; }
        int Priority { get; }
        ValidationSeverity Severity { get; }
        void Validate(T input);
    }
}
