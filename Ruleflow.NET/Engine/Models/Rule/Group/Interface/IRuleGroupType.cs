using System;

namespace Ruleflow.NET.Engine.Models.Rule.Group.Interface
{
    /// <summary>
    /// Rozhraní pro typ skupiny pravidel s typovou vazbou na vstupní data.
    /// </summary>
    /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
    public interface IRuleGroupType<TInput>
    {
        /// <summary>
        /// Unikátní identifikátor typu skupiny.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Kód typu skupiny (zkratka).
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Název typu skupiny.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Volitelný popis typu skupiny.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Indikuje, zda je typ skupiny aktivní.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Datum a čas vytvoření typu skupiny.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
    }
}