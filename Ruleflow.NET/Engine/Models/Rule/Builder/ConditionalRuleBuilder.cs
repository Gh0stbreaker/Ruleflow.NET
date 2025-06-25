using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Wrapper for conditional rules delegating to <see cref="UnifiedRuleBuilder{TInput}"/>.
    /// </summary>
    public class ConditionalRuleBuilder<TInput> : IRuleBuilder<TInput, ConditionalRuleBuilder<TInput>>
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public ConditionalRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // common configuration
        public ConditionalRuleBuilder<TInput> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public ConditionalRuleBuilder<TInput> WithName(string name) { _inner.WithName(name); return this; }
        public ConditionalRuleBuilder<TInput> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public ConditionalRuleBuilder<TInput> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public ConditionalRuleBuilder<TInput> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public ConditionalRuleBuilder<TInput> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public ConditionalRuleBuilder<TInput> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // specific configuration
        public ConditionalRuleBuilder<TInput> WithCondition(ConditionalRule<TInput>.ConditionDelegate func) { _inner.WithCondition(func); return this; }
        public ConditionalRuleBuilder<TInput> WithThenRule(IRule<TInput> rule) { _inner.WithThenRule(rule); return this; }
        public ConditionalRuleBuilder<TInput> WithElseRule(IRule<TInput> rule) { _inner.WithElseRule(rule); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }
}
