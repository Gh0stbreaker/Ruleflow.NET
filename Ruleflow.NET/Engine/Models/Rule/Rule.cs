using Ruleflow.NET.Engine.Models.Rule.Type;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;
using System.Text;

namespace Ruleflow.NET.Engine.Models.Rule
{

    /// <summary>
    /// Představuje konkrétní validační pravidlo v Ruleflow.NET systému.
    /// <para>Represents a single validation rule within the Ruleflow.NET system.</para>
    /// </summary>
    /// <typeparam name="TInput">Typ dat, která budou validována. / Type of the input being validated.</typeparam>
    public class Rule<TInput>(
        int id,
        RuleType type,
        string? ruleId = null,
        string? name = null,
        string? description = null,
        int priority = 0,
        bool isActive = true,
        DateTimeOffset? timestamp = null)
    {
        /// <summary>
        /// Interní identifikátor pravidla.
        /// <para>Internal numeric identifier of the rule.</para>
        /// </summary>
        public int Id { get; } = id;

        /// <summary>
        /// Uživatelsky definovaný identifikátor pravidla.
        /// <para>Custom rule identifier, generated if not provided.</para>
        /// </summary>
        public string RuleId { get; } = ruleId ?? Guid.NewGuid().ToString();

        /// <summary>
        /// Volitelný název pravidla.
        /// <para>Optional human readable name.</para>
        /// </summary>
        public string? Name { get; } = name;

        /// <summary>
        /// Popis pravidla.
        /// <para>Description explaining the rule purpose.</para>
        /// </summary>
        public string? Description { get; } = description;

        /// <summary>
        /// Priorita provádění pravidla.
        /// <para>Execution priority, higher means executed earlier.</para>
        /// </summary>
        public int Priority { get; } = priority;

        /// <summary>
        /// Označuje, zda je pravidlo aktivní.
        /// <para>Indicates whether the rule is active.</para>
        /// </summary>
        public bool IsActive { get; } = isActive;

        /// <summary>
        /// Čas vytvoření nebo poslední aktualizace pravidla.
        /// <para>UTC timestamp when the rule was created.</para>
        /// </summary>
        public DateTimeOffset Timestamp { get; } = timestamp?.ToUniversalTime() ?? DateTimeOffset.UtcNow;

        public int RuleTypeId { get; } = type.Id;
        public IRuleType Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Rule(Id={Id}, RuleType=\"{Type.Code}\"");
            sb.Append($", RuleId=\"{RuleId}\"");
            if (!string.IsNullOrEmpty(Name))
                sb.Append($", Name=\"{Name}\"");
            if (!string.IsNullOrEmpty(Description))
                sb.Append($", Description=\"{Description}\"");
            sb.Append($", Priority={Priority}");
            sb.Append($", IsActive={IsActive}");
            sb.Append($", Timestamp=\"{Timestamp:o}\"");
            sb.Append(")");
            return sb.ToString();
        }
    }
}