using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Models.Rule.Context;
using Ruleflow.NET.Engine.Models.Rule;
using Ruleflow.NET.Engine.Validation;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class TimeBasedRuleBuilderTests
    {
        private class Item { }

        [TestMethod]
        public void TimeBasedRule_UsesCurrentTime_WhenConfigured()
        {
            DateTimeOffset captured = DateTimeOffset.MinValue;
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("TIME", "Time");
            var rule = RuleBuilderFactory.CreateTimeBasedRuleBuilder<Item>(ruleType)
                .WithTimeCondition((i, t, ctx) => { captured = t; return true; })
                .UseCurrentTime(true)
                .Build();

            var result = rule.Evaluate(new Item(), new RuleContext("test"));
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue((DateTimeOffset.UtcNow - captured) < TimeSpan.FromSeconds(5));
        }

        [TestMethod]
        public void TimeBasedRule_RequiresEvaluationTime_WhenNotUsingCurrentTime()
        {
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("TIME", "Time");
            var rule = RuleBuilderFactory.CreateTimeBasedRuleBuilder<Item>(ruleType)
                .WithTimeCondition((i, t, ctx) => true)
                .UseCurrentTime(false)
                .Build();

            var result = rule.Evaluate(new Item(), new RuleContext("test"));
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TimeBasedRule_UsesProvidedEvaluationTime()
        {
            DateTimeOffset expected = new DateTimeOffset(2024,1,1,0,0,0,TimeSpan.Zero);
            DateTimeOffset captured = DateTimeOffset.MinValue;
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("TIME", "Time");
            var rule = RuleBuilderFactory.CreateTimeBasedRuleBuilder<Item>(ruleType)
                .WithTimeCondition((i, t, ctx) => { captured = t; return true; })
                .UseCurrentTime(false)
                .Build();

            var ctx = new RuleContext("test");
            ctx.AddParameter("EvaluationTime", expected);
            var result = rule.Evaluate(new Item(), ctx);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expected, captured);
        }
    }
}
