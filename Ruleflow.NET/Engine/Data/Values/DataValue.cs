using System;
using System.Globalization;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Interfaces;

namespace Ruleflow.NET.Engine.Data.Values
{
    /// <summary>
    /// Generic implementation of <see cref="IDataValue"/> storing a typed value.
    /// </summary>
    /// <typeparam name="T">Runtime type of the stored value.</typeparam>
    public class DataValue<T> : IDataValue
    {
        public DataType Type { get; }
        public T TypedValue { get; }
        public object? Value => TypedValue;

        public DataValue(T value, DataType type)
        {
            Type = type;
            TypedValue = value;
        }

        public bool TryGetValue<TOut>(out TOut? result)
        {
            try
            {
                if (Value is TOut direct)
                {
                    result = direct;
                    return true;
                }

                result = (TOut)Convert.ChangeType(Value!, typeof(TOut), CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
