using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Základní abstraktní třída pro pravidla s identifikátorem.
    /// <para>Base abstract class for rules that carry an identifier.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu pravidla.
    /// <para>Type of the input for the rule.</para>
    /// </typeparam>
    public abstract class IdentifiableValidationRule<T> : IValidationRule<T>
    {
        /// <summary>
        /// Jedinečný identifikátor pravidla.
        /// <para>Unique rule identifier.</para>
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Priorita provádění pravidla.
        /// <para>Execution priority.</para>
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Závažnost výsledku při selhání.
        /// <para>Severity of the validation failure.</para>
        /// </summary>
        public ValidationSeverity Severity { get; private set; }

        /// <summary>
        /// Inicializuje nové pravidlo se zadaným ID.
        /// <para>Initializes a new rule with the specified ID.</para>
        /// </summary>
        /// <param name="id">Identifikátor pravidla.</param>
        protected IdentifiableValidationRule(string id)
        {
            Id = id;
            Severity = DefaultSeverity;
        }

        /// <summary>
        /// Nastaví prioritu pravidla.
        /// <para>Sets the execution priority of the rule.</para>
        /// </summary>
        public void SetPriority(int priority) => Priority = priority;

        /// <summary>
        /// Nastaví závažnost selhání.
        /// <para>Sets the severity used when the rule fails.</para>
        /// </summary>
        public void SetSeverity(ValidationSeverity severity) => Severity = severity;

        /// <summary>
        /// Výchozí závažnost pravidla.
        /// <para>Default severity for the rule.</para>
        /// </summary>
        public abstract ValidationSeverity DefaultSeverity { get; }

        /// <summary>
        /// Provede validaci vstupu.
        /// <para>Performs validation of the input.</para>
        /// </summary>
        public abstract void Validate(T input);
    }
}
