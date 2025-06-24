using System;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    public class ConditionalValidationRule<T> : IdentifiableValidationRule<T>
    {
        private readonly Func<T, bool> _condition;
        private readonly IdentifiableValidationRule<T>? _thenRule;
        private readonly IdentifiableValidationRule<T>? _elseRule;

        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        public ConditionalValidationRule(string id, Func<T, bool> condition,
            IdentifiableValidationRule<T>? thenRule, IdentifiableValidationRule<T>? elseRule) : base(id)
        {
            _condition = condition;
            _thenRule = thenRule;
            _elseRule = elseRule;
        }

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
