using System.Collections.Generic;
using Ruleflow.NET.Engine.Data.Interfaces;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Holds mapping data during conversions.
    /// </summary>
    public class DataContext
    {
        private readonly Dictionary<string, IDataValue> _values = new();

        /// <summary>
        /// Stored values for the current mapping operation.
        /// </summary>
        public IReadOnlyDictionary<string, IDataValue> Values => _values;

        /// <summary>
        /// Stores a value in the context.
        /// </summary>
        public void Set(string key, IDataValue value)
        {
            _values[key] = value;
        }

        /// <summary>
        /// Attempts to get a value from the context.
        /// </summary>
        public bool TryGet(string key, out IDataValue? value) => _values.TryGetValue(key, out value);
    }
}
