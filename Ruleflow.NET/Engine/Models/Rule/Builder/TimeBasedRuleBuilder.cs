using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Wrapper for time based rules delegating to <see cref="UnifiedRuleBuilder{TInput}"/>.
    /// </summary>
    internal class TimeBasedRuleBuilder<TInput> : IRuleBuilder<TInput, TimeBasedRuleBuilder<TInput>>
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public TimeBasedRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // common configuration
        public TimeBasedRuleBuilder<TInput> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public TimeBasedRuleBuilder<TInput> WithName(string name) { _inner.WithName(name); return this; }
        public TimeBasedRuleBuilder<TInput> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public TimeBasedRuleBuilder<TInput> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public TimeBasedRuleBuilder<TInput> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public TimeBasedRuleBuilder<TInput> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public TimeBasedRuleBuilder<TInput> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // specific configuration
        public TimeBasedRuleBuilder<TInput> WithTimeCondition(TimeBasedRule<TInput>.TimeConditionDelegate func) { _inner.WithTimeCondition(func); return this; }
        public TimeBasedRuleBuilder<TInput> UseCurrentTime(bool useCurrent) { _inner.UseCurrentTime(useCurrent); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }
}
