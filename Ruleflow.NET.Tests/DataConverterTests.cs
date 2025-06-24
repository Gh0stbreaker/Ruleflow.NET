using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Data;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Interfaces;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class DataConverterTests
    {
        [TestMethod]
        public void TryConvert_Int32_Succeeds()
        {
            var success = DataConverter.TryConvert("123", DataType.Int32, out IDataValue? value);
            Assert.IsTrue(success);
            Assert.IsNotNull(value);
            Assert.AreEqual(DataType.Int32, value!.Type);
            Assert.IsTrue(value.TryGetValue<int>(out var result));
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TryConvert_InvalidInt32_Fails()
        {
            var success = DataConverter.TryConvert("abc", DataType.Int32, out _);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void TryConvert_Guid_Succeeds()
        {
            var guid = System.Guid.NewGuid();
            var success = DataConverter.TryConvert(guid.ToString(), DataType.Guid, out IDataValue? value);
            Assert.IsTrue(success);
            Assert.IsNotNull(value);
            Assert.AreEqual(DataType.Guid, value!.Type);
            Assert.IsTrue(value.TryGetValue<System.Guid>(out var result));
            Assert.AreEqual(guid, result);
        }
    }
}
