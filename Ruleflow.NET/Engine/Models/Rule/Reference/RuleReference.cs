using System;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Registry.Interface;

namespace Ruleflow.NET.Engine.Models.Rule.Reference
{
    /// <summary>
    /// Provides an intelligent reference to a rule. The reference keeps a weak link
    /// to the rule and can resolve it from a registry when needed.
    /// </summary>
    public sealed class RuleReference<TInput> : IRuleReference<TInput>
    {
        private WeakReference<IRule<TInput>> _weakRule;

        /// <inheritdoc />
        public string RuleId { get; }

        /// <summary>
        /// Creates a reference from the specified rule.
        /// </summary>
        public RuleReference(IRule<TInput> rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            _weakRule = new WeakReference<IRule<TInput>>(rule);
            RuleId = rule.RuleId;
        }

        /// <inheritdoc />
        public bool TryResolve(IRuleRegistry<TInput> registry, out IRule<TInput>? rule)
        {
            if (_weakRule.TryGetTarget(out rule))
                return true;

            if (registry != null)
            {
                rule = registry.GetRuleById(RuleId);
                if (rule != null)
                {
                    _weakRule.SetTarget(rule);
                    return true;
                }
            }

            rule = null;
            return false;
        }
    }
}
