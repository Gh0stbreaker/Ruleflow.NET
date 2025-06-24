using System;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    public class ActionValidationRule<T> : IdentifiableValidationRule<T>
    {
        private readonly Action<T> _action;
        public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

        public ActionValidationRule(string id, Action<T> action) : base(id)
        {
            _action = action;
        }

        public override void Validate(T input)
        {
            _action(input);
        }
    }
}
