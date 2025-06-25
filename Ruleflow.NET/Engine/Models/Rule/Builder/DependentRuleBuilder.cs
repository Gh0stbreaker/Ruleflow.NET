using System.Collections.Generic;
using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Wrapper for dependent rules delegating to <see cref="UnifiedRuleBuilder{TInput}"/>.
    /// </summary>
    public class DependentRuleBuilder<TInput> : IRuleBuilder<TInput, DependentRuleBuilder<TInput>>
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public DependentRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // common configuration
        public DependentRuleBuilder<TInput> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public DependentRuleBuilder<TInput> WithName(string name) { _inner.WithName(name); return this; }
        public DependentRuleBuilder<TInput> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public DependentRuleBuilder<TInput> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public DependentRuleBuilder<TInput> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public DependentRuleBuilder<TInput> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public DependentRuleBuilder<TInput> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // specific configuration
        public DependentRuleBuilder<TInput> WithEvaluationFunction(DependentRule<TInput>.EvaluateDependenciesDelegate func) { _inner.WithEvaluationFunction(func); return this; }
        public DependentRuleBuilder<TInput> AddDependency(IRule<TInput> rule) { _inner.AddDependency(rule); return this; }
        public DependentRuleBuilder<TInput> AddDependencies(IEnumerable<IRule<TInput>> rules) { _inner.AddDependencies(rules); return this; }
        public DependentRuleBuilder<TInput> RequireAllDependencies() { _inner.RequireAllDependencies(); return this; }
        public DependentRuleBuilder<TInput> RequireAnyDependency() { _inner.RequireAnyDependency(); return this; }
        public DependentRuleBuilder<TInput> RequireMinimumDependencies(int minimum) { _inner.RequireMinimumDependencies(minimum); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }
}
