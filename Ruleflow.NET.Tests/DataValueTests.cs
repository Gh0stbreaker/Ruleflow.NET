using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Values;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class DataValueTests
    {
        [TestMethod]
        public void TryGetValue_ReturnsTypedValue()
        {
            var value = new DataValue<int>(42, DataType.Int32);
            Assert.IsTrue(value.TryGetValue<int>(out var result));
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TryGetValue_InvalidCast_ReturnsFalse()
        {
            var value = new DataValue<string>("foo", DataType.String);
            Assert.IsFalse(value.TryGetValue<int>(out _));
        }
    }
}
