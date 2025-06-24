using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    /// <summary>
    /// Pomocná třída pro sestavení jednoduchého pravidla.
    /// <para>Helper class for building a simple action-based rule.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
    public class SimpleRuleBuilder<T>
    {
        private string _id = Guid.NewGuid().ToString();
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;
        private Action<T>? _action;

        /// <summary>
        /// Nastaví identifikátor pravidla.
        /// <para>Sets the rule identifier.</para>
        /// </summary>
        public SimpleRuleBuilder<T> WithId(string id) { _id = id; return this; }

        /// <summary>
        /// Nastaví prioritu pravidla.
        /// <para>Sets the rule priority.</para>
        /// </summary>
        public SimpleRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }

        /// <summary>
        /// Nastaví závažnost selhání.
        /// <para>Sets the severity of validation failure.</para>
        /// </summary>
        public SimpleRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

        /// <summary>
        /// Definuje akci prováděnou pravidlem.
        /// <para>Defines the action executed by the rule.</para>
        /// </summary>
        public SimpleRuleBuilder<T> WithAction(Action<T> act) { _action = act; return this; }

        /// <summary>
        /// Vytvoří instanci pravidla z nastavených parametrů.
        /// <para>Builds the rule instance from configured parameters.</para>
        /// </summary>
        public ActionValidationRule<T> Build()
        {
            if (_action == null) throw new InvalidOperationException("Action not set");
            var rule = new ActionValidationRule<T>(_id, _action);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
