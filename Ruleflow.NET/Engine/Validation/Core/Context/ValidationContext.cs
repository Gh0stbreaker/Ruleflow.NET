using System.Collections.Generic;

namespace Ruleflow.NET.Engine.Validation.Core.Context
{
    public class RuleExecutionResult
    {
        public bool Success { get; set; }
    }

    public class ValidationContext
    {
        private static readonly ValidationContext _instance = new ValidationContext();
        public static ValidationContext Instance => _instance;

        public Dictionary<string, object> Properties { get; } = new();
        public Dictionary<string, RuleExecutionResult> RuleResults { get; } = new();

        public void Clear()
        {
            Properties.Clear();
            RuleResults.Clear();
        }
    }
}
