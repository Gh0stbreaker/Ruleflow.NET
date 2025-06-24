using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Results
{
    public class ValidationError
    {
        public string Message { get; }
        public ValidationSeverity Severity { get; }
        public string? Code { get; }
        public object? Context { get; }

        public ValidationError(string message, ValidationSeverity severity, string? code = null, object? context = null)
        {
            Message = message ?? throw new System.ArgumentNullException(nameof(message));
            Severity = severity;
            Code = code;
            Context = context;
        }
    }
}
