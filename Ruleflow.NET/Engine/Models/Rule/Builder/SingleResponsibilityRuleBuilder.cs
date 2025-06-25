using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Wrapper builder for single responsibility rules delegating to <see cref="UnifiedRuleBuilder{TInput}"/>.
    /// </summary>
    public class SingleResponsibilityRuleBuilder<TInput> : IRuleBuilder<TInput, SingleResponsibilityRuleBuilder<TInput>>
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public SingleResponsibilityRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // Common configuration delegates to inner builder
        public SingleResponsibilityRuleBuilder<TInput> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithName(string name) { _inner.WithName(name); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public SingleResponsibilityRuleBuilder<TInput> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // Specific configuration
        public SingleResponsibilityRuleBuilder<TInput> WithValidation(SingleResponsibilityRule<TInput>.ValidateDelegate func) { _inner.WithValidation(func); return this; }
        public SingleResponsibilityRuleBuilder<TInput> WithErrorMessage(string message) { _inner.WithErrorMessage(message); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }
}
