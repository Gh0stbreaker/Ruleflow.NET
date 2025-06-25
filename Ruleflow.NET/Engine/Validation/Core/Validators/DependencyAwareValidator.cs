using System;
using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Core.Context;
using Ruleflow.NET.Engine.Validation.Core.Results;
using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Engine.Validation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ruleflow.NET.Engine.Validation.Core.Validators
{
    public class DependencyAwareValidator<T> : IValidator<T>
    {
        private readonly Dictionary<string, IValidationRule<T>> _rules;
        private readonly ILogger<DependencyAwareValidator<T>> _logger;

        public DependencyAwareValidator(IEnumerable<IValidationRule<T>> rules, ILogger<DependencyAwareValidator<T>>? logger = null)
        {
            _rules = rules.ToDictionary(r => r.Id);
            _logger = logger ?? NullLogger<DependencyAwareValidator<T>>.Instance;
            DetectCycles();
        }

        private void DetectCycles()
        {
            var visiting = new HashSet<string>();
            var visited = new HashSet<string>();

            bool Visit(string id)
            {
                if (visiting.Contains(id)) return true;
                if (!visited.Add(id)) return false;
                visiting.Add(id);
                if (_rules[id] is DependentValidationRule<T> dep)
                {
                    foreach (var d in dep.Dependencies)
                    {
                        if (_rules.ContainsKey(d) && Visit(d)) return true;
                    }
                }
                visiting.Remove(id);
                return false;
            }

            foreach (var id in _rules.Keys)
                if (Visit(id))
                    throw new InvalidOperationException("Circular dependency detected");
        }

        private ValidationResult ExecuteRule(IValidationRule<T> rule, T input, Dictionary<string, ValidationResult> cache)
        {
            if (cache.TryGetValue(rule.Id, out var cached)) return cached;

            var context = ValidationContext.Instance;
            var result = new ValidationResult();

            if (rule is DependentValidationRule<T> dep)
            {
                var depResults = dep.Dependencies.Select(id => ExecuteRule(_rules[id], input, cache)).ToList();
                bool allSuccess = depResults.All(r => r.IsValid);
                bool anySuccess = depResults.Any(r => r.IsValid);
                bool anyFailure = depResults.Any(r => !r.IsValid);
                bool shouldExecute = dep.DependencyType switch
                {
                    DependencyType.RequiresAllSuccess => allSuccess,
                    DependencyType.RequiresAnyFailure => anyFailure,
                    DependencyType.RequiresAnySuccess => anySuccess,
                    _ => true
                };
                if (!shouldExecute)
                {
                    context.RuleResults[rule.Id] = new RuleExecutionResult { Success = true };
                    cache[rule.Id] = result;
                    return result;
                }
            }

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

            cache[rule.Id] = result;
            return result;
        }

        public ValidationResult CollectValidationResults(T input)
        {
            _logger.LogInformation("Starting validation of {InputType}", typeof(T).Name);
            var cache = new Dictionary<string, ValidationResult>();
            var final = new ValidationResult();
            foreach (var rule in _rules.Values.OrderByDescending(r => r.Priority))
            {
                var res = ExecuteRule(rule, input, cache);
                final.AddErrors(res.Errors);
            }
            _logger.LogInformation("Finished validation of {InputType}", typeof(T).Name);
            return final;
        }

        public bool IsValid(T input) => CollectValidationResults(input).IsValid;
        public ValidationError? GetFirstError(T input) => CollectValidationResults(input).Errors.FirstOrDefault();
        public void ValidateOrThrow(T input) => CollectValidationResults(input).ThrowIfInvalid();
        public ValidationResult ValidateAndProcess(T input, Action<T> action)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) action(input);
            return res;
        }
        public void ValidateAndExecute(T input, Action successAction, Action<IReadOnlyList<ValidationError>> failureAction)
        {
            var res = CollectValidationResults(input);
            if (res.IsValid) successAction(); else failureAction(res.Errors);
        }
    }
}
