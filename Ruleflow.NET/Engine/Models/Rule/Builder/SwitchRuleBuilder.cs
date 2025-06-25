using System;
using Ruleflow.NET.Engine.Models.Rule.Builder.Interface;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Wrapper for switch rules delegating to <see cref="UnifiedRuleBuilder{TInput}"/>.
    /// </summary>
    public class SwitchRuleBuilder<TInput> : IRuleBuilder<TInput, SwitchRuleBuilder<TInput>>
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public SwitchRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // common configuration
        public SwitchRuleBuilder<TInput> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public SwitchRuleBuilder<TInput> WithName(string name) { _inner.WithName(name); return this; }
        public SwitchRuleBuilder<TInput> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public SwitchRuleBuilder<TInput> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public SwitchRuleBuilder<TInput> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public SwitchRuleBuilder<TInput> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public SwitchRuleBuilder<TInput> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // specific configuration
        public SwitchRuleBuilder<TInput> WithSwitchKeyFunction(SwitchRule<TInput>.SwitchKeyDelegate func) { _inner.WithSwitchKeyFunction(func); return this; }
        public SwitchRuleBuilder<TInput> AddCase(object key, IRule<TInput> rule) { _inner.AddCase(key, rule); return this; }
        public SwitchRuleBuilder<TInput> WithDefaultCase(IRule<TInput> rule) { _inner.WithDefaultCase(rule); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }

    /// <summary>
    /// Typed switch rule builder delegating to the unified builder.
    /// </summary>
    public class SwitchRuleBuilder<TInput, TKey> : IRuleBuilder<TInput, SwitchRuleBuilder<TInput, TKey>> where TKey : notnull
    {
        private readonly UnifiedRuleBuilder<TInput> _inner;

        public SwitchRuleBuilder(int id, IRuleType<TInput> type)
        {
            _inner = new UnifiedRuleBuilder<TInput>(id, type);
        }

        // common configuration
        public SwitchRuleBuilder<TInput, TKey> WithRuleId(string ruleId) { _inner.WithRuleId(ruleId); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithName(string name) { _inner.WithName(name); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithDescription(string description) { _inner.WithDescription(description); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithPriority(int priority) { _inner.WithPriority(priority); return this; }
        public SwitchRuleBuilder<TInput, TKey> SetActive(bool isActive) { _inner.SetActive(isActive); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithTimestamp(DateTimeOffset timestamp) { _inner.WithTimestamp(timestamp); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithType(IRuleType<TInput> type) { _inner.WithType(type); return this; }

        // specific configuration
        public SwitchRuleBuilder<TInput, TKey> WithSwitchKeyFunction(Func<TInput, Rule.Context.RuleContext, TKey> func)
        {
            _inner.WithSwitchKeyFunction((i, c) => func(i, c));
            return this;
        }
        public SwitchRuleBuilder<TInput, TKey> AddCase(TKey key, IRule<TInput> rule) { _inner.AddCase(key!, rule); return this; }
        public SwitchRuleBuilder<TInput, TKey> WithDefaultCase(IRule<TInput> rule) { _inner.WithDefaultCase(rule); return this; }

        public Rule<TInput> Build() => _inner.Build();
    }
}
