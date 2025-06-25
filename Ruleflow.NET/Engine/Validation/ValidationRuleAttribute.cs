using System;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation
{
    /// <summary>
    /// Attribute for marking static validation methods to be loaded automatically.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidationRuleAttribute : Attribute
    {
        public string Id { get; }
        public int Priority { get; init; }
        public ValidationSeverity Severity { get; init; } = ValidationSeverity.Error;

        public ValidationRuleAttribute(string id)
        {
            Id = id;
        }
    }
}
