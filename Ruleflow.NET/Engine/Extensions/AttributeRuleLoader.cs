using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Mapping;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Core.Base;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Engine.Extensions
{
    /// <summary>
    /// Utility for loading mapping and validation rules from custom attributes.
    /// </summary>
    public static class AttributeRuleLoader
    {
        /// <summary>
        /// Scans type <typeparamref name="T"/> for properties decorated with
        /// <see cref="MapKeyAttribute"/> and creates corresponding mapping rules.
        /// </summary>
        public static IEnumerable<DataMappingRule<T>> LoadMappingRules<T>()
        {
            var rules = new List<DataMappingRule<T>>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var attr = prop.GetCustomAttribute<MapKeyAttribute>();
                if (attr == null) continue;

                var param = Expression.Parameter(typeof(T), "x");
                var access = Expression.Property(param, prop);
                Expression body = Expression.Convert(access, typeof(object));
                var lambda = Expression.Lambda<Func<T, object?>>(body, param);
                rules.Add(new DataMappingRule<T>(lambda, attr.Key, attr.Type, attr.Required));
            }
            return rules;
        }

        /// <summary>
        /// Scans the assembly of type <typeparamref name="T"/> for static methods
        /// decorated with <see cref="ValidationRuleAttribute"/> that accept a single
        /// parameter of type <typeparamref name="T"/>.
        /// </summary>
        public static IEnumerable<IValidationRule<T>> LoadValidationRules<T>()
        {
            var rules = new List<IValidationRule<T>>();
            var assembly = typeof(T).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var attr = method.GetCustomAttribute<ValidationRuleAttribute>();
                    if (attr == null) continue;
                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(T))
                        continue;

                    if (!typeof(void).Equals(method.ReturnType))
                        continue;

                    var action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), method);
                    var rule = new ActionValidationRule<T>(attr.Id, action);
                    rule.SetPriority(attr.Priority);
                    rule.SetSeverity(attr.Severity);
                    rules.Add(rule);
                }
            }
            return rules;
        }
    }
}
