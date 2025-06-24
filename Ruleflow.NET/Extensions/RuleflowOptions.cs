using System.Collections.Generic;
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
    }
}
