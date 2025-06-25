using System;
using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Core.Context;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ruleflow.NET.Engine.Validation.Core.Validators
{
    /// <summary>
    /// Basic validator that executes a list of rules sequentially.
    /// </summary>
    public class Validator<T> : IValidator<T>
    {
        private readonly List<IValidationRule<T>> _rules;
        private readonly ILogger<Validator<T>> _logger;

        public Validator(IEnumerable<IValidationRule<T>> rules, ILogger<Validator<T>>? logger = null)
        {
            _rules = rules.ToList();
            _logger = logger ?? NullLogger<Validator<T>>.Instance;
        }

        /// <summary>
        /// Executes all configured rules and collects their validation results.
        /// </summary>
        public ValidationResult CollectValidationResults(T input)
        {
            _logger.LogInformation("Starting validation of {InputType}", typeof(T).Name);
            var context = ValidationContext.Instance;
            var result = new ValidationResult();
            foreach (var rule in _rules.OrderByDescending(r => r.Priority))
            {
                try
                {
                    rule.Validate(input);
                    context.RuleResults[rule.Id] = new RuleExecutionResult { Success = true };
                    _logger.LogDebug("Rule {RuleId} executed successfully", rule.Id);
                }
                catch (Exception ex)
                {
                    context.RuleResults[rule.Id] = new RuleExecutionResult { Success = false };
                    result.AddError(ex.Message, rule.Severity);
                    _logger.LogError(ex, "Rule {RuleId} failed: {Message}", rule.Id, ex.Message);
                }
            }
            _logger.LogInformation("Finished validation of {InputType}", typeof(T).Name);
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

        /// <summary>
        /// Validates the input and, if successful, invokes the provided processing action.
        /// </summary>
        public ValidationResult ValidateAndProcess(T input, Action<T> processingAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid)
                processingAction(input);
            return res;
        }

        /// <summary>
        /// Validates the input and executes either success or failure action based on the result.
        /// </summary>
        public void ValidateAndExecute(T input, Action successAction, Action<IReadOnlyList<ValidationError>> failureAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) successAction(); else failureAction(res.Errors);
        }
    }
}
