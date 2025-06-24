using System;
using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Results
{
    public class ValidationResult
    {
        private readonly List<ValidationError> _errors = new();
        public IReadOnlyList<ValidationError> Errors => _errors;

        public bool IsValid => !_errors.Any(e => e.Severity >= ValidationSeverity.Error);
        public bool HasCriticalErrors => _errors.Any(e => e.Severity == ValidationSeverity.Critical);

        public void AddError(ValidationError error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            _errors.Add(error);
        }

        public void AddError(string message, ValidationSeverity severity, string? code = null, object? context = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _errors.Add(new ValidationError(message, severity, code, context));
        }

        public void AddErrors(IEnumerable<ValidationError> errors)
        {
            if (errors == null) throw new NullReferenceException();
            foreach (var e in errors) AddError(e);
        }

        public IEnumerable<ValidationError> GetErrorsBySeverity(ValidationSeverity severity)
        {
            return _errors.Where(e => e.Severity == severity);
        }

        public ValidationResult OnSuccess(Action action)
        {
            if (IsValid) action();
            return this;
        }

        public ValidationResult OnSuccess<T>(T input, Action<T> action)
        {
            if (IsValid) action(input);
            return this;
        }

        public ValidationResult OnFailure(Action<IReadOnlyList<ValidationError>> action)
        {
            if (!IsValid) action(Errors);
            return this;
        }

        public void ThrowIfInvalid()
        {
            if (!IsValid)
            {
                var ex = new AggregateException("Validace selhala s kritickými chybami");
                throw ex;
            }
        }
    }
}
