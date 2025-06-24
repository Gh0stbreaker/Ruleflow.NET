using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation.Core.Validators
{
    public class CompositeValidator<T> : IValidator<T>
    {
        private readonly List<IValidator<T>> _validators;

        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators.ToList();
        }

        public ValidationResult CollectValidationResults(T input)
        {
            var result = new ValidationResult();
            foreach (var v in _validators)
            {
                var r = v.CollectValidationResults(input);
                result.AddErrors(r.Errors);
            }
            return result;
        }

        public bool IsValid(T input) => CollectValidationResults(input).IsValid;
        public ValidationError? GetFirstError(T input) => CollectValidationResults(input).Errors.FirstOrDefault();
        public void ValidateOrThrow(T input) => CollectValidationResults(input).ThrowIfInvalid();
        public ValidationResult ValidateAndProcess(T input, System.Action<T> action)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) action(input);
            return res;
        }
        public void ValidateAndExecute(T input, System.Action successAction, System.Action<IReadOnlyList<ValidationError>> failureAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) successAction(); else failureAction(res.Errors);
        }
    }
}
