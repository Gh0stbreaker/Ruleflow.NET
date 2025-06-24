using Ruleflow.NET.Engine.Validation.Core.Results;

namespace Ruleflow.NET.Engine.Validation.Interfaces
{
    public interface IValidator<T>
    {
        ValidationResult CollectValidationResults(T input);
        bool IsValid(T input);
        ValidationError? GetFirstError(T input);
        void ValidateOrThrow(T input);
        ValidationResult ValidateAndProcess(T input, System.Action<T> processingAction);
        void ValidateAndExecute(T input, System.Action successAction, System.Action<System.Collections.Generic.IReadOnlyList<ValidationError>> failureAction);
    }
}
