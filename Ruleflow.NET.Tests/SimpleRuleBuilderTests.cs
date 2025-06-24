using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Enums;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class SimpleRuleBuilderTests
    {
        private class Item { public int Qty { get; set; } }

        [TestMethod]
        public void Build_WithConfiguredValues_CreatesRule()
        {
            var rule = RuleflowExtensions.CreateRule<Item>()
                .WithId("test")
                .WithPriority(5)
                .WithSeverity(ValidationSeverity.Warning)
                .WithAction(i => { })
                .Build();

            Assert.AreEqual("test", rule.Id);
            Assert.AreEqual(5, rule.Priority);
            Assert.AreEqual(ValidationSeverity.Warning, rule.Severity);
        }

        [TestMethod]
        public void Build_WithoutAction_Throws()
        {
            var builder = RuleflowExtensions.CreateRule<Item>();
            Assert.ThrowsException<System.InvalidOperationException>(() => builder.Build());
        }
    }
}
