using System;
using System.Collections.Generic;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data;
using Ruleflow.NET.Engine.Data.Interfaces;
using Ruleflow.NET.Engine.Data.Values;
using Ruleflow.NET.Engine.Extensions;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Generic strict automapper for converting between dictionaries and objects.
    /// </summary>
    public class DataAutoMapper<T> : IDataAutoMapper<T>
    {
        private readonly List<DataMappingRule<T>> _rules;

        /// <summary>
        /// Creates a mapper using mapping rules discovered from
        /// <see cref="MapKeyAttribute"/> attributes on <typeparamref name="T"/>.
        /// </summary>
        public static DataAutoMapper<T> FromAttributes()
        {
            var profile = Engine.Extensions.AttributeRuleLoader.LoadProfile<T>();
            return new DataAutoMapper<T>(profile.MappingRules);
        }

        /// <summary>
        /// Creates a new mapper with the provided rules.
        /// </summary>
        public DataAutoMapper(IEnumerable<DataMappingRule<T>> rules)
        {
            _rules = new List<DataMappingRule<T>>(rules);
        }

        /// <summary>
        /// Maps dictionary data to an object instance.
        /// </summary>
        public T MapToObject(IDictionary<string, string> data, DataContext context)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var obj = Activator.CreateInstance<T>();

            foreach (var rule in _rules)
            {
                if (!data.TryGetValue(rule.Key, out var raw))
                {
                    if (rule.IsRequired)
                        throw new DataMappingException($"Required key '{rule.Key}' missing.");
                    continue;
                }

                if (!DataConverter.TryConvert(raw, rule.Type, out var value) || value == null)
                    throw new DataMappingException($"Failed to convert key '{rule.Key}' to type {rule.Type}.");

                rule.Property.SetValue(obj, value.Value);
                context.Set(rule.Key, value);
            }

            return obj;
        }

        /// <summary>
        /// Maps an object instance to dictionary data.
        /// </summary>
        public IDictionary<string, IDataValue> MapToData(T obj, DataContext context)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var result = new Dictionary<string, IDataValue>();

            foreach (var rule in _rules)
            {
                var raw = rule.Property.GetValue(obj);
                if (raw == null)
                {
                    if (rule.IsRequired)
                        throw new DataMappingException($"Required property '{rule.Property.Name}' is null.");
                    continue;
                }

                var value = CreateValue(raw, rule.Type);
                result[rule.Key] = value;
                context.Set(rule.Key, value);
            }

            return result;
        }

        private static IDataValue CreateValue(object raw, DataType type)
        {
            return type switch
            {
                DataType.String => new DataValue<string>((string)raw, type),
                DataType.Int32 => new DataValue<int>(Convert.ToInt32(raw), type),
                DataType.Int64 => new DataValue<long>(Convert.ToInt64(raw), type),
                DataType.Decimal => new DataValue<decimal>(Convert.ToDecimal(raw), type),
                DataType.Double => new DataValue<double>(Convert.ToDouble(raw), type),
                DataType.Boolean => new DataValue<bool>(Convert.ToBoolean(raw), type),
                DataType.DateTime => new DataValue<DateTime>(Convert.ToDateTime(raw), type),
                DataType.Guid => new DataValue<Guid>((Guid)raw, type),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }
}
