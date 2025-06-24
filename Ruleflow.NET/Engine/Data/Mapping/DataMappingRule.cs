using System;
using System.Linq.Expressions;
using System.Reflection;
using Ruleflow.NET.Engine.Data.Enums;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Defines mapping information for a single property.
    /// </summary>
    public class DataMappingRule<T>
    {
        /// <summary>
        /// Key used in the data dictionary.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Expected data type.
        /// </summary>
        public DataType Type { get; }

        /// <summary>
        /// Indicates whether the value is required.
        /// </summary>
        public bool IsRequired { get; }

        internal PropertyInfo Property { get; }

        /// <summary>
        /// Creates a new mapping rule for the specified property.
        /// </summary>
        public DataMappingRule(Expression<Func<T, object?>> property, string key, DataType type, bool isRequired = false)
        {
            if (property.Body is MemberExpression member)
                Property = (PropertyInfo)member.Member;
            else if (property.Body is UnaryExpression u && u.Operand is MemberExpression m)
                Property = (PropertyInfo)m.Member;
            else
                throw new ArgumentException("Invalid property expression", nameof(property));

            Key = key;
            Type = type;
            IsRequired = isRequired;
        }
    }
}
