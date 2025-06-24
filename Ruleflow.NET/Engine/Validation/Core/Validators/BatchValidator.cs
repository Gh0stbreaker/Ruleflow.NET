using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation.Core.Validators
{
    /// <summary>
    /// Validator that processes a batch of inputs sequentially.
    /// </summary>
    public class BatchValidator<T> : IValidator<IEnumerable<T>>
    {
        private readonly Validator<T> _validator;
        private readonly ReaderWriterLockSlim _lock = new();

        public BatchValidator(IEnumerable<IValidationRule<T>> rules)
        {
            _validator = new Validator<T>(rules);
        }

        /// <summary>
        /// Validates each input in the batch and aggregates all validation errors.
        /// </summary>
        public ValidationResult CollectValidationResults(IEnumerable<T> inputs)
        {
            var result = new ValidationResult();
            _lock.EnterWriteLock();
            try
            {
                foreach (var input in inputs)
                {
                    var res = _validator.CollectValidationResults(input);
                    result.AddErrors(res.Errors);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return result;
        }

        public bool IsValid(IEnumerable<T> inputs) => CollectValidationResults(inputs).IsValid;

        public ValidationError? GetFirstError(IEnumerable<T> inputs)
        {
            return CollectValidationResults(inputs).Errors.FirstOrDefault();
        }

        public void ValidateOrThrow(IEnumerable<T> inputs)
        {
            CollectValidationResults(inputs).ThrowIfInvalid();
        }

        public ValidationResult ValidateAndProcess(IEnumerable<T> inputs, System.Action<IEnumerable<T>> processingAction)
        {
            var res = CollectValidationResults(inputs);
            if (res.IsValid)
                processingAction(inputs);
            return res;
        }

        public void ValidateAndExecute(IEnumerable<T> inputs, System.Action successAction, System.Action<IReadOnlyList<ValidationError>> failureAction)
        {
            var res = CollectValidationResults(inputs);
            if (res.IsValid) successAction(); else failureAction(res.Errors);
        }
    }
}
