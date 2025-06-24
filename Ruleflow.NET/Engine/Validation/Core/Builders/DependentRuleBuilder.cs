using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    /// <summary>
    /// Builder pro pravidlo závislé na jiných pravidlech.
    /// <para>Builder for a rule that depends on other rules.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
    public class DependentRuleBuilder<T>
    {
        private readonly string _id;
        private readonly List<string> _dependencies = new();
        private Action<T>? _action;
        private DependencyType _dependencyType = DependencyType.RequiresAllSuccess;
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;

        /// <summary>
        /// Inicializuje builder s identifikátorem pravidla.
        /// <para>Initializes the builder with a rule identifier.</para>
        /// </summary>
        public DependentRuleBuilder(string id)
        {
            _id = id;
        }

        /// <summary>
        /// Přidá závislosti na ostatní pravidla.
        /// <para>Adds dependencies on other rules.</para>
        /// </summary>
        public DependentRuleBuilder<T> DependsOn(params string[] ids)
        {
            _dependencies.AddRange(ids);
            return this;
        }

        /// <summary>
        /// Nastaví typ závislosti mezi pravidly.
        /// <para>Sets the dependency type.</para>
        /// </summary>
        public DependentRuleBuilder<T> WithDependencyType(DependencyType type)
        {
            _dependencyType = type;
            return this;
        }

        /// <summary>
        /// Definuje akci prováděnou pravidlem.
        /// <para>Defines the action executed by the rule.</para>
        /// </summary>
        public DependentRuleBuilder<T> WithAction(Action<T> action) { _action = action; return this; }

        /// <summary>
        /// Nastaví prioritu pravidla.
        /// <para>Sets the rule priority.</para>
        /// </summary>
        public DependentRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }

        /// <summary>
        /// Nastaví závažnost při selhání.
        /// <para>Sets failure severity.</para>
        /// </summary>
        public DependentRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

        /// <summary>
        /// Vytvoří instanci závislého pravidla.
        /// <para>Builds the dependent validation rule.</para>
        /// </summary>
        public DependentValidationRule<T> Build()
        {
            if (_action == null) throw new InvalidOperationException("Action not set");
            var rule = new DependentValidationRule<T>(_id, _action);
            rule.AddDependencies(_dependencies);
            rule.SetDependencyType(_dependencyType);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
