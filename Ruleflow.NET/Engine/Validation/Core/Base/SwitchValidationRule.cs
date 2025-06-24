using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Pravidlo volící různé podpravidlo podle klíče.
    /// <para>Rule that selects a sub-rule to execute based on a key.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
    /// <typeparam name="TKey">Typ klíče pro výběr.</typeparam>
    public class SwitchValidationRule<T, TKey> : IdentifiableValidationRule<T>
        where TKey : notnull
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly Dictionary<TKey, IdentifiableValidationRule<T>> _cases;
        private readonly IdentifiableValidationRule<T>? _defaultRule;

        /// <inheritdoc />
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        /// <summary>
        /// Inicializuje switch pravidlo s danými případy.
        /// <para>Initializes the switch rule with the provided cases.</para>
        /// </summary>
        public SwitchValidationRule(string id, Func<T, TKey> selector,
            Dictionary<TKey, IdentifiableValidationRule<T>> cases,
            IdentifiableValidationRule<T>? defaultRule) : base(id)
        {
            _keySelector = selector;
            _cases = cases;
            _defaultRule = defaultRule;
        }

        /// <summary>
        /// Vybere a spustí pravidlo podle klíče z vstupu.
        /// <para>Selects and executes a rule based on the key derived from the input.</para>
        /// </summary>
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
