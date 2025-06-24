using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation
{
    public static class ValidationExtensions
    {
        public static ValidationResult Validate<T>(this T input, IEnumerable<IValidationRule<T>> rules)
        {
            var validator = new Core.Validators.Validator<T>(rules);
            return validator.CollectValidationResults(input);
        }

        public static ValidationResult ValidateBatch<T>(this IEnumerable<T> inputs, IEnumerable<IValidationRule<T>> rules)
        {
            var validator = new Core.Validators.BatchValidator<T>(rules);
            return validator.CollectValidationResults(inputs);
        }
    }
}
