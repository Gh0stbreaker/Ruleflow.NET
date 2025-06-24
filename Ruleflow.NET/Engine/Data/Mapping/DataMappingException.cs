using System;

namespace Ruleflow.NET.Engine.Data.Mapping
{
    /// <summary>
    /// Exception thrown when strict mapping rules are violated.
    /// </summary>
    public class DataMappingException : Exception
    {
        public DataMappingException(string message) : base(message) { }
    }
}
