using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Builders
{
    public class DependentRuleBuilder<T>
    {
        private readonly string _id;
        private readonly List<string> _dependencies = new();
        private Action<T>? _action;
        private DependencyType _dependencyType = DependencyType.RequiresAllSuccess;
        private int _priority;
        private ValidationSeverity _severity = ValidationSeverity.Error;

        public DependentRuleBuilder(string id)
        {
            _id = id;
        }

        public DependentRuleBuilder<T> DependsOn(params string[] ids)
        {
            _dependencies.AddRange(ids);
            return this;
        }

        public DependentRuleBuilder<T> WithDependencyType(DependencyType type)
        {
            _dependencyType = type;
            return this;
        }

        public DependentRuleBuilder<T> WithAction(Action<T> action) { _action = action; return this; }
        public DependentRuleBuilder<T> WithPriority(int p) { _priority = p; return this; }
        public DependentRuleBuilder<T> WithSeverity(ValidationSeverity s) { _severity = s; return this; }

        public DependentValidationRule<T> Build()
        {
            if (_action == null) throw new InvalidOperationException("Action not set");
            var rule = new DependentValidationRule<T>(_id, _action);
            rule.AddDependencies(_dependencies);
            rule.SetDependencyType(_dependencyType);
            rule.SetPriority(_priority);
            rule.SetSeverity(_severity);
            return rule;
        }
    }
}
