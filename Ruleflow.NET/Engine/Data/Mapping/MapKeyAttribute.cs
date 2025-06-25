using System;
using Ruleflow.NET.Engine.Data.Enums;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Attribute used to mark properties for automatic data mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MapKeyAttribute : Attribute
    {
        /// <summary>
        /// Key used in the data dictionary.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Expected data type of the value.
        /// </summary>
        public DataType Type { get; }

        /// <summary>
        /// Indicates whether the value is required.
        /// </summary>
        public bool Required { get; init; }

        public MapKeyAttribute(string key, DataType type)
        {
            Key = key;
            Type = type;
        }
    }
}
