using System;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Pravidlo vyhodnocující podmínku a volící odpovídající větev.
    /// <para>Validation rule that evaluates a condition and executes the appropriate branch.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.
    /// <para>Type of the input.</para>
    /// </typeparam>
    public class ConditionalValidationRule<T> : IdentifiableValidationRule<T>
    {
        private readonly Func<T, bool> _condition;
        private readonly IdentifiableValidationRule<T>? _thenRule;
        private readonly IdentifiableValidationRule<T>? _elseRule;

        /// <inheritdoc />
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        /// <summary>
        /// Inicializuje podmíněné pravidlo.
        /// <para>Initializes the conditional rule.</para>
        /// </summary>
        public ConditionalValidationRule(string id, Func<T, bool> condition,
            IdentifiableValidationRule<T>? thenRule, IdentifiableValidationRule<T>? elseRule) : base(id)
        {
            _condition = condition;
            _thenRule = thenRule;
            _elseRule = elseRule;
        }

        /// <summary>
        /// Vyhodnotí vstup a spustí větev THEN nebo ELSE.
        /// <para>Evaluates the input and executes the THEN or ELSE branch.</para>
        /// </summary>
        public override void Validate(T input)
        {
            if (_condition(input))
            {
                _thenRule?.Validate(input);
            }
            else
            {
                _elseRule?.Validate(input);
            }
        }
    }
}
