using System.Collections.Generic;
using System.Reflection;
using Ruleflow.NET.Engine.Models.Rule.Interface;

namespace Ruleflow.NET.Extensions
{
    /// <summary>
    /// Options used when configuring Ruleflow services.
    /// </summary>
    public class RuleflowOptions<TInput>
    {
        /// <summary>
        /// Initial rules that will be added to the registry.
        /// </summary>
        public IEnumerable<IRule<TInput>>? InitialRules { get; set; }

        /// <summary>
        /// If true, a default <see cref="IValidator{TInput}"/> built from registered rules will be registered.
        /// </summary>
        public bool RegisterDefaultValidator { get; set; }

        /// <summary>
        /// When true, rules and mapping information will be automatically loaded
        /// from attributes discovered via reflection.
        /// </summary>
        public bool AutoRegisterAttributeRules { get; set; }

        /// <summary>
        /// Assemblies that will be scanned for <see cref="ValidationRuleAttribute"/>
        /// declarations. If null, only the assembly containing <typeparamref name="TInput"/>
        /// will be scanned.
        /// </summary>
        public IEnumerable<Assembly>? AssembliesToScan { get; set; }

        /// <summary>
        /// Optional namespace prefixes used to filter discovered types when
        /// scanning for rules.
        /// </summary>
        public IEnumerable<string>? NamespaceFilters { get; set; }
    }
}
