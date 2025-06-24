using System;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    /// <summary>
    /// Pravidlo vykonávající zadanou akci jako validaci.
    /// <para>Validation rule that executes a provided action.</para>
    /// </summary>
    /// <typeparam name="T">Typ vstupu.
    /// <para>Type of the input.</para>
    /// </typeparam>
    public class ActionValidationRule<T> : IdentifiableValidationRule<T>
    {
        private readonly Action<T> _action;

        /// <inheritdoc />
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        /// <summary>
        /// Vytvoří nové pravidlo na základě delegáta akce.
        /// <para>Creates a new rule based on an action delegate.</para>
        /// </summary>
        public ActionValidationRule(string id, Action<T> action) : base(id)
        {
            _action = action;
        }

        /// <summary>
        /// Provede akci nad vstupem.
        /// <para>Executes the stored action on the input.</para>
        /// </summary>
        public override void Validate(T input)
        {
            _action(input);
        }
    }
}
