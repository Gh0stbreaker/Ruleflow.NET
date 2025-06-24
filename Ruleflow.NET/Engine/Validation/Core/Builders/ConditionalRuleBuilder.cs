using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    public class ConditionalRuleBuilder<T>
    {
        private readonly Func<T, bool> _condition;
        private readonly SimpleRuleBuilder<T> _thenBuilder = new();
        private readonly SimpleRuleBuilder<T> _elseBuilder = new();
        private bool _hasThen;
        private bool _hasElse;
        private string _id = Guid.NewGuid().ToString();
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;

        public ConditionalRuleBuilder(Func<T, bool> condition)
        {
            _condition = condition;
        }

        public ConditionalRuleBuilder<T> WithId(string id) { _id = id; return this; }
        public ConditionalRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }
        public ConditionalRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }
        public ConditionalRuleBuilder<T> Then(Action<SimpleRuleBuilder<T>> cfg) { _hasThen=true; cfg(_thenBuilder); return this; }
        public ConditionalRuleBuilder<T> Else(Action<SimpleRuleBuilder<T>> cfg) { _hasElse=true; cfg(_elseBuilder); return this; }

        public ConditionalValidationRule<T> Build()
        {
            var thenRule = _hasThen ? _thenBuilder.Build() : null;
            var elseRule = _hasElse ? _elseBuilder.Build() : null;
            var rule = new ConditionalValidationRule<T>(_id, _condition, thenRule, elseRule);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
