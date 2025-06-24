using System;
using System.Globalization;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Interfaces;
using Ruleflow.NET.Engine.Data.Values;

namespace Ruleflow.NET.Engine.Data
{
    /// <summary>
    /// Utility class providing safe conversions of raw values into <see cref="IDataValue"/> instances.
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Attempts to convert a string representation to a strongly typed <see cref="IDataValue"/>.
        /// </summary>
        /// <param name="input">Raw string input.</param>
        /// <param name="type">Requested data type.</param>
        /// <param name="value">Resulting data value when conversion succeeds.</param>
        /// <returns>True when conversion succeeded.</returns>
        public static bool TryConvert(string input, DataType type, out IDataValue? value)
        {
            value = null;
            switch (type)
            {
                case DataType.String:
                    value = new DataValue<string>(input, DataType.String);
                    return true;
                case DataType.Int32:
                    if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i32))
                    {
                        value = new DataValue<int>(i32, DataType.Int32);
                        return true;
                    }
                    return false;
                case DataType.Int64:
                    if (long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i64))
                    {
                        value = new DataValue<long>(i64, DataType.Int64);
                        return true;
                    }
                    return false;
                case DataType.Decimal:
                    if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var dec))
                    {
                        value = new DataValue<decimal>(dec, DataType.Decimal);
                        return true;
                    }
                    return false;
                case DataType.Double:
                    if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var dbl))
                    {
                        value = new DataValue<double>(dbl, DataType.Double);
                        return true;
                    }
                    return false;
                case DataType.Boolean:
                    if (bool.TryParse(input, out var b))
                    {
                        value = new DataValue<bool>(b, DataType.Boolean);
                        return true;
                    }
                    return false;
                case DataType.DateTime:
                    if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    {
                        value = new DataValue<DateTime>(dt, DataType.DateTime);
                        return true;
                    }
                    return false;
                case DataType.Guid:
                    if (Guid.TryParse(input, out var g))
                    {
                        value = new DataValue<Guid>(g, DataType.Guid);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}
