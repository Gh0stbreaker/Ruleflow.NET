using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Registry.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Reference
{
    /// <summary>
    /// Represents a lightweight reference to a rule that can be resolved via a registry.
    /// </summary>
    public interface IRuleReference<TInput>
    {
        /// <summary>
        /// Identifier of the referenced rule.
        /// </summary>
        string RuleId { get; }

        /// <summary>
        /// Attempts to resolve the referenced rule from the provided registry.
        /// </summary>
        /// <param name="registry">Registry used to resolve the rule.</param>
        /// <param name="rule">Resolved rule instance if found.</param>
        /// <returns>True if the rule was resolved.</returns>
        bool TryResolve(IRuleRegistry<TInput> registry, out IRule<TInput>? rule);
    }
}
