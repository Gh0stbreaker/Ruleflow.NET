using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    public class SwitchRuleBuilder<T, TKey>
        where TKey : notnull
    {
        private readonly Func<T, TKey> _selector;
        private readonly Dictionary<TKey, SimpleRuleBuilder<T>> _cases = new();
        private SimpleRuleBuilder<T>? _defaultBuilder;
        private bool _hasDefault;
        private string _id = Guid.NewGuid().ToString();
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;

        public SwitchRuleBuilder(Func<T, TKey> selector)
        {
            _selector = selector;
        }

        public SwitchRuleBuilder<T, TKey> WithId(string id) { _id = id; return this; }
        public SwitchRuleBuilder<T, TKey> WithPriority(int p) { _priority = p; return this; }
        public SwitchRuleBuilder<T, TKey> WithSeverity(ValidationSeverity s) { _severity = s; return this; }
        public SwitchRuleBuilder<T, TKey> Case(TKey key, Action<SimpleRuleBuilder<T>> cfg)
        {
            var b = new SimpleRuleBuilder<T>();
            cfg(b);
            _cases[key] = b;
            return this;
        }
        public SwitchRuleBuilder<T, TKey> Default(Action<SimpleRuleBuilder<T>> cfg)
        {
            _defaultBuilder = new SimpleRuleBuilder<T>();
            cfg(_defaultBuilder);
            _hasDefault = true;
            return this;
        }

        public SwitchValidationRule<T, TKey> Build()
        {
            var builtCases = new Dictionary<TKey, IdentifiableValidationRule<T>>();
            foreach (var kv in _cases)
                builtCases[kv.Key] = kv.Value.Build();
            var defaultRule = _hasDefault ? _defaultBuilder!.Build() : null;
            var rule = new SwitchValidationRule<T, TKey>(_id, _selector, builtCases, defaultRule);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
