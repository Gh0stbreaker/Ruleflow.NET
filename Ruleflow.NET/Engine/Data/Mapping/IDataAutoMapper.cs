using System.Collections.Generic;
using Ruleflow.NET.Engine.Data.Interfaces;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Interface for the strict automapper.
    /// </summary>
    public interface IDataAutoMapper<T>
    {
        /// <summary>
        /// Converts dictionary data to an object.
        /// </summary>
        T MapToObject(IDictionary<string, string> data, DataContext context);

        /// <summary>
        /// Converts an object to dictionary data.
        /// </summary>
        IDictionary<string, IDataValue> MapToData(T obj, DataContext context);
    }
}
