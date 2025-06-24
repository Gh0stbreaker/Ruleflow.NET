using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Validation.Core.Base
{
    public class DependentValidationRule<T> : ActionValidationRule<T>
    {
        public IReadOnlyList<string> Dependencies => _dependencies;
        private readonly List<string> _dependencies = new();
        public DependencyType DependencyType { get; private set; } = DependencyType.RequiresAllSuccess;

        public DependentValidationRule(string id, Action<T> action) : base(id, action)
        {
        }

        public void AddDependencies(IEnumerable<string> ids)
        {
            _dependencies.AddRange(ids);
        }

        public void SetDependencyType(DependencyType type) => DependencyType = type;
    }
}
