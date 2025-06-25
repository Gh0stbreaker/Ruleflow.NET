using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Models.Rule;
using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Models.Rule.Context;
using System;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class RuleContextTests
    {
        private class Dummy { }

        [TestMethod]
        public void Constructor_SetsNameDescriptionAndTimestamp()
        {
            var before = DateTimeOffset.UtcNow;
            var ctx = new RuleContext("test", "desc");
            Assert.AreEqual("test", ctx.Name);
            Assert.AreEqual("desc", ctx.Description);
            Assert.IsTrue((ctx.Timestamp - before) < TimeSpan.FromSeconds(5));
        }

        [TestMethod]
        public void AddParameter_AddsAndUpdatesValues()
        {
            var ctx = new RuleContext("ctx");
            ctx.AddParameter("k", 1);
            Assert.AreEqual(1, ctx.Parameters["k"]);
            ctx.AddParameter("k", 2);
            Assert.AreEqual(2, ctx.Parameters["k"]);
            Assert.AreEqual(1, ctx.Parameters.Count);
        }

        [TestMethod]
        public void ToString_IncludesNameAndParameterCount()
        {
            var ctx = new RuleContext("ctx");
            ctx.AddParameter("x", 123);
            var str = ctx.ToString();
            Assert.IsTrue(str.Contains("RuleContext"));
            Assert.IsTrue(str.Contains("ctx"));
            Assert.IsTrue(str.Contains("Params=1"));
        }

        [TestMethod]
        public void GenericContext_PopulatesRuleMetadata()
        {
            var type = RuleBuilderFactory.CreateRuleType<Dummy>("S", "Single");
            var rule = RuleBuilderFactory.CreateUnifiedRuleBuilder<Dummy>(type)
                .WithValidation((d, c) => true)
                .WithErrorMessage("err")
                .Build();

            var ctx = new RuleContext<Dummy>(rule);
            Assert.AreEqual(rule.RuleId, ctx.Parameters["RuleId"]);
            Assert.AreEqual(rule.Priority, ctx.Parameters["RulePriority"]);
            Assert.AreEqual(rule.IsActive, ctx.Parameters["RuleIsActive"]);
            Assert.AreEqual(rule.Timestamp, ctx.Parameters["RuleTimestamp"]);
            Assert.AreEqual(rule.Name, ctx.Name);
        }
    }
}
