using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Interfaces
{
    /// <summary>
    /// Definuje společné vlastnosti pro validační pravidlo.
    /// <para>Defines the common members of a validation rule.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu, který pravidlo zpracovává.
    /// <para>Type of the input validated by the rule.</para>
    /// </typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        /// Jedinečný identifikátor pravidla.
        /// <para>Unique identifier of the rule.</para>
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Priorita provádění pravidla.
        /// <para>Execution priority of the rule.</para>
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Závažnost výsledku při selhání.
        /// <para>Severity used when the rule fails.</para>
        /// </summary>
        ValidationSeverity Severity { get; }

        /// <summary>
        /// Provede samotnou validaci vstupu.
        /// <para>Performs the validation of the provided input.</para>
        /// </summary>
        /// <param name="input">Vstupní data.</param>
        void Validate(T input);
    }
}
