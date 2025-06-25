using System.Collections.Generic;
using Ruleflow.NET.Engine.Data.Mapping;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Extensions
{
    /// <summary>
    /// Container for mapping and validation rules that can be registered as a single unit.
    /// </summary>
    /// <typeparam name="T">Type the rules operate on.</typeparam>
    public class RuleflowProfile<T>
    {
        /// <summary>
        /// Mapping rules contained in this profile.
        /// </summary>
        public List<DataMappingRule<T>> MappingRules { get; } = new();

        /// <summary>
        /// Validation rules contained in this profile.
        /// </summary>
        public List<IValidationRule<T>> ValidationRules { get; } = new();

        /// <summary>
        /// Creates an empty profile.
        /// </summary>
        public RuleflowProfile() { }

        /// <summary>
        /// Creates a profile from the provided rules.
        /// </summary>
        public RuleflowProfile(IEnumerable<DataMappingRule<T>> mappings, IEnumerable<IValidationRule<T>> validations)
        {
            if (mappings != null)
                MappingRules.AddRange(mappings);
            if (validations != null)
                ValidationRules.AddRange(validations);
        }
    }
}
