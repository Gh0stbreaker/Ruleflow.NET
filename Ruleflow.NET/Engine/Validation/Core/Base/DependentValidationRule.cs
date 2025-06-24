using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Pravidlo závislé na výsledcích jiných pravidel.
    /// <para>Validation rule that depends on other rule results.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.</typeparam>
    public class DependentValidationRule<T> : ActionValidationRule<T>
    {
        /// <summary>
        /// Seznam ID pravidel, na která toto pravidlo závisí.
        /// <para>IDs of rules that this rule depends on.</para>
        /// </summary>
        public IReadOnlyList<string> Dependencies => _dependencies;
        private readonly List<string> _dependencies = new();

        /// <summary>
        /// Typ závislosti mezi pravidly.
        /// <para>Type of dependency between rules.</para>
        /// </summary>
        public DependencyType DependencyType { get; private set; } = DependencyType.RequiresAllSuccess;

        /// <summary>
        /// Vytvoří nové závislé pravidlo.
        /// <para>Creates a new dependent rule.</para>
        /// </summary>
        public DependentValidationRule(string id, Action<T> action) : base(id, action)
        {
        }

        /// <summary>
        /// Přidá závislosti na další pravidla.
        /// <para>Adds dependencies to other rules.</para>
        /// </summary>
        public void AddDependencies(IEnumerable<string> ids)
        {
            _dependencies.AddRange(ids);
        }

        /// <summary>
        /// Nastaví typ závislosti.
        /// <para>Sets the dependency type.</para>
        /// </summary>
        public void SetDependencyType(DependencyType type) => DependencyType = type;
    }
}
