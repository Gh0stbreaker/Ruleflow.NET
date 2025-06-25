using System;
using System.Collections.Generic;
using System.Linq;
using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;
using Ruleflow.NET.Engine.Models.Rule;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Unified builder that can configure any supported rule type.
    /// </summary>
    public class UnifiedRuleBuilder<TInput> : RuleBuilder<TInput, UnifiedRuleBuilder<TInput>>
    {
        // Single responsibility
        private SingleResponsibilityRule<TInput>.ValidateDelegate? _validateFunc;
        private string? _errorMessage;

        // Conditional
        private ConditionalRule<TInput>.ConditionDelegate? _conditionFunc;
        private IRule<TInput>? _thenRule;
        private IRule<TInput>? _elseRule;

        // Dependent
        private DependentRule<TInput>.EvaluateDependenciesDelegate? _evaluateFunc;
        private readonly List<IRule<TInput>> _dependencies = new();

        // Switch
        private SwitchRule<TInput>.SwitchKeyDelegate? _switchKeyFunc;
        private readonly Dictionary<object, IRule<TInput>> _cases = new();
        private IRule<TInput>? _defaultCase;

        // Time based
        private TimeBasedRule<TInput>.TimeConditionDelegate? _timeConditionFunc;
        private bool _useCurrentTime = true;

        public UnifiedRuleBuilder(int id, IRuleType<TInput> type) : base(id, type)
        {
        }

        // Single responsibility methods
        public UnifiedRuleBuilder<TInput> WithValidation(SingleResponsibilityRule<TInput>.ValidateDelegate func)
        {
            _validateFunc = func;
            return this;
        }

        public UnifiedRuleBuilder<TInput> WithErrorMessage(string message)
        {
            _errorMessage = message;
            return this;
        }

        // Conditional methods
        public UnifiedRuleBuilder<TInput> WithCondition(ConditionalRule<TInput>.ConditionDelegate func)
        {
            _conditionFunc = func;
            return this;
        }

        public UnifiedRuleBuilder<TInput> WithThenRule(IRule<TInput> rule)
        {
            _thenRule = rule;
            return this;
        }

        public UnifiedRuleBuilder<TInput> WithElseRule(IRule<TInput> rule)
        {
            _elseRule = rule;
            return this;
        }

        // Dependent methods
        public UnifiedRuleBuilder<TInput> WithEvaluationFunction(DependentRule<TInput>.EvaluateDependenciesDelegate func)
        {
            _evaluateFunc = func;
            return this;
        }

        public UnifiedRuleBuilder<TInput> AddDependency(IRule<TInput> rule)
        {
            _dependencies.Add(rule);
            return this;
        }

        public UnifiedRuleBuilder<TInput> AddDependencies(IEnumerable<IRule<TInput>> rules)
        {
            foreach (var r in rules)
                _dependencies.Add(r);
            return this;
        }

        public UnifiedRuleBuilder<TInput> RequireAllDependencies()
        {
            _evaluateFunc = (i, c, results) => results.All(r => r.IsSuccess);
            return this;
        }

        public UnifiedRuleBuilder<TInput> RequireAnyDependency()
        {
            _evaluateFunc = (i, c, results) => results.Any(r => r.IsSuccess);
            return this;
        }

        public UnifiedRuleBuilder<TInput> RequireMinimumDependencies(int minimumCount)
        {
            _evaluateFunc = (i, c, results) => results.Count(r => r.IsSuccess) >= minimumCount;
            return this;
        }

        // Switch methods
        public UnifiedRuleBuilder<TInput> WithSwitchKeyFunction(SwitchRule<TInput>.SwitchKeyDelegate func)
        {
            _switchKeyFunc = func;
            return this;
        }

        public UnifiedRuleBuilder<TInput> AddCase(object key, IRule<TInput> rule)
        {
            _cases[key] = rule;
            return this;
        }

        public UnifiedRuleBuilder<TInput> WithDefaultCase(IRule<TInput> rule)
        {
            _defaultCase = rule;
            return this;
        }

        // Time based methods
        public UnifiedRuleBuilder<TInput> WithTimeCondition(TimeBasedRule<TInput>.TimeConditionDelegate func)
        {
            _timeConditionFunc = func;
            return this;
        }

        public UnifiedRuleBuilder<TInput> UseCurrentTime(bool useCurrent)
        {
            _useCurrentTime = useCurrent;
            return this;
        }

        public override Rule<TInput> Build()
        {
            if (_switchKeyFunc != null)
            {
                return new SwitchRule<TInput>(Id, Type, _switchKeyFunc, _defaultCase, _cases, RuleId, Name ?? "Unnamed Switch Rule", Description, Priority, IsActive, Timestamp);
            }

            if (_timeConditionFunc != null)
            {
                return new TimeBasedRule<TInput>(Id, Type, _timeConditionFunc, _useCurrentTime, RuleId, Name, Description, Priority, IsActive, Timestamp);
            }

            if (_conditionFunc != null)
            {
                if (_conditionFunc == null)
                    throw new InvalidOperationException("Condition must be provided.");
                return new ConditionalRule<TInput>(Id, Type, _conditionFunc, _thenRule, _elseRule, RuleId, Name ?? "Unnamed Conditional Rule", Description, Priority, IsActive, Timestamp);
            }

            if (_evaluateFunc != null || _dependencies.Count > 0)
            {
                if (_evaluateFunc == null)
                    throw new InvalidOperationException("Evaluation function must be provided for dependent rule.");
                return new DependentRule<TInput>(Id, Type, _evaluateFunc, _dependencies, RuleId, Name ?? "Unnamed Dependent Rule", Description, Priority, IsActive, Timestamp);
            }

            if (_validateFunc != null)
            {
                return new SingleResponsibilityRule<TInput>(Id, Type, _validateFunc, RuleId, Name ?? "Unnamed Single Responsibility Rule", Description, Priority, IsActive, Timestamp);
            }

            throw new InvalidOperationException("No rule configuration specified.");
        }
    }
}
