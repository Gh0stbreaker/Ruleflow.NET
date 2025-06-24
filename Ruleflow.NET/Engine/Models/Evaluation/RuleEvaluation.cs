using System;
using System.Collections.Generic;
using System.Text;
using Ruleflow.NET.Engine.Models.Shared;

namespace Ruleflow.NET.Engine.Models.Evaluation
{
    /// <summary>
    /// Uchovává výsledek vyhodnocení sdíleného pravidla pro daný vstup.
    /// <para>Holds the evaluation result for a shared rule and a particular input.</para>
    /// </summary>
    /// <typeparam name="TInput">Typ validovaných dat. / Type of validated input.</typeparam>
    public class RuleEvaluation<TInput>(
        SharedRule<TInput> sharedRule,
        TInput input,
        bool passed,
        Dictionary<string, object>? details = null,
        DateTimeOffset? evaluatedAt = null)
    {
        public Guid EvaluationId { get; } = Guid.NewGuid();
        /// <summary>
        /// Reference na sdílené pravidlo, které bylo vyhodnoceno.
        /// <para>Reference to the shared rule instance that was evaluated.</para>
        /// </summary>
        public SharedRule<TInput> SharedRule { get; } = sharedRule ?? throw new ArgumentNullException(nameof(sharedRule));
        /// <summary>
        /// Původní vstup, který byl předán do pravidla.
        /// <para>The original input passed to the rule.</para>
        /// </summary>
        public TInput Input { get; } = input;
        /// <summary>
        /// Výsledek vyhodnocení (true = pravidlo prošlo).
        /// <para>Indicates whether the rule passed.</para>
        /// </summary>
        public bool Passed { get; } = passed;
        /// <summary>
        /// Detailní informace z vyhodnocení (např. chybové hlášky, mezivýsledky).
        /// <para>Additional details collected during evaluation.</para>
        /// </summary>
        public IReadOnlyDictionary<string, object>? Details { get; } = details;
        /// <summary>
        /// Čas provedení vyhodnocení (UTC).
        /// <para>Timestamp of the evaluation in UTC.</para>
        /// </summary>
        public DateTimeOffset EvaluatedAt { get; } = evaluatedAt?.ToUniversalTime() ?? DateTimeOffset.UtcNow;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"RuleEvaluation(Id={EvaluationId}, Rule={SharedRule.Rule.RuleId}, Passed={Passed}");
            sb.Append($", Priority={SharedRule.Rule.Priority}");
            sb.Append($", IsActive={SharedRule.Rule.IsActive}");
            sb.Append($", ContextId={SharedRule.Context.ContextId}");
            sb.Append($", EvaluatedAt=\"{EvaluatedAt:o}\"");
            if (Details != null && Details.Count > 0)
                sb.Append($", DetailsCount={Details.Count}");
            sb.Append(")");
            return sb.ToString();
        }
    }
}