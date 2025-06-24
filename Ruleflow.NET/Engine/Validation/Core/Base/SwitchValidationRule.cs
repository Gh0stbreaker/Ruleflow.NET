using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    public class SwitchValidationRule<T, TKey> : IdentifiableValidationRule<T>
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly Dictionary<TKey, IdentifiableValidationRule<T>> _cases;
        private readonly IdentifiableValidationRule<T>? _defaultRule;
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        public SwitchValidationRule(string id, Func<T, TKey> selector,
            Dictionary<TKey, IdentifiableValidationRule<T>> cases,
            IdentifiableValidationRule<T>? defaultRule) : base(id)
        {
            _keySelector = selector;
            _cases = cases;
            _defaultRule = defaultRule;
        }

        public override void Validate(T input)
        {
            var key = _keySelector(input);
            if (_cases.TryGetValue(key, out var rule))
                rule.Validate(input);
            else
                _defaultRule?.Validate(input);
        }
    }
}
