using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    /// <summary>
    /// Builder pro tvorbu podmíněného pravidla.
    /// <para>Builder used to create a conditional validation rule.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
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

        /// <summary>
        /// Nastaví identifikátor pravidla.
        /// <para>Sets the identifier of the resulting rule.</para>
        /// </summary>
        public ConditionalRuleBuilder<T> WithId(string id) { _id = id; return this; }

        /// <summary>
        /// Nastaví prioritu pravidla.
        /// <para>Sets the priority of the rule.</para>
        /// </summary>
        public ConditionalRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }

        /// <summary>
        /// Nastaví závažnost selhání.
        /// <para>Sets the failure severity.</para>
        /// </summary>
        public ConditionalRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

        /// <summary>
        /// Definuje větev THEN.
        /// <para>Defines the THEN branch.</para>
        /// </summary>
        public ConditionalRuleBuilder<T> Then(Action<SimpleRuleBuilder<T>> cfg) { _hasThen=true; cfg(_thenBuilder); return this; }

        /// <summary>
        /// Definuje větev ELSE.
        /// <para>Defines the ELSE branch.</para>
        /// </summary>
        public ConditionalRuleBuilder<T> Else(Action<SimpleRuleBuilder<T>> cfg) { _hasElse=true; cfg(_elseBuilder); return this; }

        /// <summary>
        /// Vytvoří instanci podmíněného pravidla.
        /// <para>Builds the conditional validation rule.</para>
        /// </summary>
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
