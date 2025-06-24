using System;
using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Core.Context;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation.Core.Validators
{
    public class Validator<T> : IValidator<T>
    {
        private readonly List<IValidationRule<T>> _rules;

        public Validator(IEnumerable<IValidationRule<T>> rules)
        {
            _rules = rules.ToList();
        }

        public ValidationResult CollectValidationResults(T input)
        {
            var context = ValidationContext.Instance;
            var result = new ValidationResult();
            foreach (var rule in _rules.OrderByDescending(r => r.Priority))
            {
                try
                {
                    rule.Validate(input);
                    context.RuleResults[rule.Id] = new RuleExecutionResult { Success = true };
                }
                catch (Exception ex)
                {
                    context.RuleResults[rule.Id] = new RuleExecutionResult { Success = false };
                    result.AddError(ex.Message, rule.Severity);
                }
            }
            return result;
        }

        public bool IsValid(T input) => CollectValidationResults(input).IsValid;

        public ValidationError? GetFirstError(T input)
        {
            return CollectValidationResults(input).Errors.FirstOrDefault();
        }

        public void ValidateOrThrow(T input)
        {
            CollectValidationResults(input).ThrowIfInvalid();
        }

        public ValidationResult ValidateAndProcess(T input, Action<T> processingAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid)
                processingAction(input);
            return res;
        }

        public void ValidateAndExecute(T input, Action successAction, Action<IReadOnlyList<ValidationError>> failureAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) successAction(); else failureAction(res.Errors);
        }
    }
}
