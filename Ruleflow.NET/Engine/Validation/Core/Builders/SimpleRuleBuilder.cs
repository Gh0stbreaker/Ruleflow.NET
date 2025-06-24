using System;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    public class SimpleRuleBuilder<T>
    {
        private string _id = Guid.NewGuid().ToString();
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;
        private Action<T>? _action;

        public SimpleRuleBuilder<T> WithId(string id) { _id = id; return this; }
        public SimpleRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }
        public SimpleRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }
        public SimpleRuleBuilder<T> WithAction(Action<T> act) { _action = act; return this; }

        public ActionValidationRule<T> Build()
        {
            if (_action == null) throw new InvalidOperationException("Action not set");
            var rule = new ActionValidationRule<T>(_id, _action);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
