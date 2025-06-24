using Ruleflow.NET.Engine.Data.Enums;

namespace Ruleflow.NET.Engine.Data.Interfaces
{
    /// <summary>
    /// Represents a strongly typed value used within Ruleflow.NET.
    /// </summary>
    public interface IDataValue
    {
        /// <summary>
        /// Type of the underlying value.
        /// </summary>
        DataType Type { get; }

        /// <summary>
        /// Value boxed as object.
        /// </summary>
        object? Value { get; }

        /// <summary>
        /// Attempts to obtain the value as a specific type.
        /// </summary>
        /// <typeparam name="T">Requested type.</typeparam>
        /// <param name="result">Converted value if successful.</param>
        /// <returns>True when conversion succeeded.</returns>
        bool TryGetValue<T>(out T? result);
    }
}
