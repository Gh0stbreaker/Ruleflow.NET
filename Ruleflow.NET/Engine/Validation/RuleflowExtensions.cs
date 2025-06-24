using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Core.Builders;

namespace Ruleflow.NET.Engine.Validation
{
    public static class RuleflowExtensions
    {
        public static SimpleRuleBuilder<T> CreateRule<T>() => new SimpleRuleBuilder<T>();
        public static DependentRuleBuilder<T> CreateDependentRule<T>(string id) => new DependentRuleBuilder<T>(id);
        public static ConditionalRuleBuilder<T> CreateConditionalRule<T>(Func<T, bool> predicate) => new ConditionalRuleBuilder<T>(predicate);
        public static SwitchRuleBuilder<T, TKey> CreateSwitchRule<T, TKey>(Func<T, TKey> selector)
            where TKey : notnull => new SwitchRuleBuilder<T, TKey>(selector);
        public static EventTriggerRuleBuilder<T> CreateEventRule<T>(string eventName) =>
            new EventTriggerRuleBuilder<T>().WithEvent(eventName);
    }
}
