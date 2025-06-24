using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Models.Rule;
using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Models.Rule.Context;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class SwitchRuleBuilderTests
    {
        private class Item { public string Category { get; set; } = string.Empty; }

        [TestMethod]
        public void SwitchRule_UsesMatchingCase()
        {
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("SWITCH", "Switch");
            var caseRule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Item>(ruleType)
                .WithValidation((i, ctx) => true)
                .WithErrorMessage("case")
                .Build();
            var defaultRule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Item>(ruleType)
                .WithValidation((i, ctx) => false)
                .WithErrorMessage("default")
                .Build();

            var switchRule = RuleBuilderFactory.CreateSwitchRuleBuilder<Item>(ruleType)
                .WithSwitchKeyFunction((i, ctx) => i.Category)
                .AddCase("A", caseRule)
                .WithDefaultCase(defaultRule)
                .Build();

            var ctx = new RuleContext("test");
            var result = switchRule.Evaluate(new Item { Category = "A" }, ctx);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void SwitchRule_UsesDefaultCaseWhenNoMatch()
        {
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("SWITCH", "Switch");
            var caseRule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Item>(ruleType)
                .WithValidation((i, ctx) => true)
                .WithErrorMessage("case")
                .Build();
            var defaultRule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Item>(ruleType)
                .WithValidation((i, ctx) => false)
                .WithErrorMessage("default")
                .Build();

            var switchRule = RuleBuilderFactory.CreateSwitchRuleBuilder<Item>(ruleType)
                .WithSwitchKeyFunction((i, ctx) => i.Category)
                .AddCase("A", caseRule)
                .WithDefaultCase(defaultRule)
                .Build();

            var ctx = new RuleContext("test");
            var result = switchRule.Evaluate(new Item { Category = "B" }, ctx);
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TypedSwitchRule_WorksCorrectly()
        {
            var ruleType = RuleBuilderFactory.CreateRuleType<Item>("TSWITCH", "TypedSwitch");
            var caseRule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Item>(ruleType)
                .WithValidation((i, ctx) => true)
                .WithErrorMessage("case")
                .Build();

            var switchRule = RuleBuilderFactory.CreateSwitchRuleBuilder<Item, int>(ruleType)
                .WithSwitchKeyFunction((i, ctx) => i.Category.Length)
                .AddCase(1, caseRule)
                .Build();

            var ctx = new RuleContext("test");
            var result = switchRule.Evaluate(new Item { Category = "X" }, ctx);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
