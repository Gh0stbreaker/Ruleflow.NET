using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Models.Rule.Type;
using Ruleflow.NET.Engine.Models.Rule.Type.Interface;
using System;

namespace Ruleflow.NET.Engine.Models.Rule.Builder
{
    /// <summary>
    /// Tovární třída pro vytváření builderů pravidel v Ruleflow.NET.
    /// </summary>
    public static class RuleBuilderFactory
    {
        private static int _nextRuleId = 1;

        /// <summary>
        /// Generuje další dostupné ID pro pravidlo.
        /// </summary>
        /// <returns>Nové ID pravidla.</returns>
        private static int GetNextRuleId() => _nextRuleId++;

        /// <summary>
        /// Vytvoří jednoduchý RuleType pro použití v builderech.
        /// </summary>
        /// <param name="code">Kód typu pravidla.</param>
        /// <param name="name">Název typu pravidla.</param>
        /// <param name="description">Popis typu pravidla.</param>
        /// <returns>Vytvořený RuleType.</returns>
        public static RuleType<TInput> CreateRuleType<TInput>(string code, string name, string? description = null)
        {
            return new RuleType<TInput>(
                GetNextRuleId(),
                code,
                name,
                description,
                true,
                DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Vytvoří builder pro jednoduché pravidlo s jednou odpovědností.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro jednoduché pravidlo.</returns>
        public static SingleResponsibilityRuleBuilder<TInput> CreateSingleResponsibilityRuleBuilder<TInput>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("SINGLE", "Single Responsibility Rule", "Pravidlo s jednou odpovědností");
            return new SingleResponsibilityRuleBuilder<TInput>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro podmínkové pravidlo.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro podmínkové pravidlo.</returns>
        public static ConditionalRuleBuilder<TInput> CreateConditionalRuleBuilder<TInput>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("CONDITIONAL", "Conditional Rule", "Podmínkové pravidlo");
            return new ConditionalRuleBuilder<TInput>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro pravidlo závislé na jiných pravidlech.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro závislé pravidlo.</returns>
        public static DependentRuleBuilder<TInput> CreateDependentRuleBuilder<TInput>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("DEPENDENT", "Dependent Rule", "Pravidlo závislé na jiných pravidlech");
            return new DependentRuleBuilder<TInput>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro přepínací pravidlo.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro přepínací pravidlo.</returns>
        public static SwitchRuleBuilder<TInput> CreateSwitchRuleBuilder<TInput>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("SWITCH", "Switch Rule", "Přepínací pravidlo");
            return new SwitchRuleBuilder<TInput>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro přepínací pravidlo se silně typovaným klíčem.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <typeparam name="TKey">Typ klíče pro přepínání.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro přepínací pravidlo se silně typovaným klíčem.</returns>
        public static SwitchRuleBuilder<TInput, TKey> CreateSwitchRuleBuilder<TInput, TKey>(RuleType<TInput>? ruleType = null)
            where TKey : notnull
        {
            ruleType ??= CreateRuleType<TInput>("TYPED_SWITCH", "Typed Switch Rule", "Typované přepínací pravidlo");
            return new SwitchRuleBuilder<TInput, TKey>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro prediktivní pravidlo.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <typeparam name="THistoryData">Typ historických dat používaných pro predikci.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro prediktivní pravidlo.</returns>
        public static PredictiveRuleBuilder<TInput, THistoryData> CreatePredictiveRuleBuilder<TInput, THistoryData>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("PREDICTIVE", "Predictive Rule", "Prediktivní pravidlo");
            return new PredictiveRuleBuilder<TInput, THistoryData>(GetNextRuleId(), ruleType);
        }

        /// <summary>
        /// Vytvoří builder pro časové pravidlo.
        /// </summary>
        /// <typeparam name="TInput">Typ validovaných dat.</typeparam>
        /// <param name="ruleType">Typ pravidla (volitelné).</param>
        /// <returns>Builder pro časové pravidlo.</returns>
        public static TimeBasedRuleBuilder<TInput> CreateTimeBasedRuleBuilder<TInput>(RuleType<TInput>? ruleType = null)
        {
            ruleType ??= CreateRuleType<TInput>("TIME_BASED", "Time Based Rule", "Časově závislé pravidlo");
            return new TimeBasedRuleBuilder<TInput>(GetNextRuleId(), ruleType);
        }
    }
}