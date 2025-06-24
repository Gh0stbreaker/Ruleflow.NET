using Ruleflow.NET.Engine.Validation.Core.Results;

namespace Ruleflow.NET.Engine.Validation.Interfaces
{
    /// <summary>
    /// Rozhraní pro validátory zajišťující vyhodnocení pravidel.
    /// <para>Interface for validators that evaluate validation rules.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupních dat.
    /// <para>Type of the validated input.</para>
    /// </typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Sesbírá detailní výsledky validace.
        /// <para>Collects detailed validation results.</para>
        /// </summary>
        /// <param name="input">Validovaný objekt.</param>
        ValidationResult CollectValidationResults(T input);

        /// <summary>
        /// Zjistí, zda je objekt platný.
        /// <para>Checks if the input is valid.</para>
        /// </summary>
        bool IsValid(T input);

        /// <summary>
        /// Vrátí první nalezenou chybu.
        /// <para>Returns the first validation error if any.</para>
        /// </summary>
        ValidationError? GetFirstError(T input);

        /// <summary>
        /// Validuje vstup a v případě chyby vyhodí výjimku.
        /// <para>Validates the input and throws if invalid.</para>
        /// </summary>
        void ValidateOrThrow(T input);

        /// <summary>
        /// Validuje vstup a následně provede zpracování.
        /// <para>Validates the input and then performs processing.</para>
        /// </summary>
        ValidationResult ValidateAndProcess(T input, System.Action<T> processingAction);

        /// <summary>
        /// Validuje vstup a provede akce pro úspěch či selhání.
        /// <para>Validates the input and executes success or failure actions.</para>
        /// </summary>
        void ValidateAndExecute(T input, System.Action successAction, System.Action<System.Collections.Generic.IReadOnlyList<ValidationError>> failureAction);
    }
}
