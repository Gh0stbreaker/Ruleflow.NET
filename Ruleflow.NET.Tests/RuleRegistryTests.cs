using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Models.Rule;
using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Models.Rule.Builder;
using Ruleflow.NET.Engine.Registry;
using Ruleflow.NET.Engine.Registry.Interface;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class RuleRegistryTests
    {
        private class Dummy { public int Value { get; set; } }

        [TestMethod]
        public void RegisterRule_AddsRuleToRegistry()
        {
            var registry = new RuleRegistry<Dummy>();
            var rule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Dummy>()
                .WithValidation((d, ctx) => d.Value > 0)
                .WithErrorMessage("Invalid")
                .Build();

            var added = registry.RegisterRule(rule);
            Assert.IsTrue(added);
            Assert.AreEqual(1, registry.Count);
            Assert.AreSame(rule, registry.GetRuleById(rule.RuleId));
        }

        [TestMethod]
        public void UnregisterRule_RemovesRuleFromRegistry()
        {
            var registry = new RuleRegistry<Dummy>();
            var rule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Dummy>()
                .WithValidation((d, ctx) => d.Value > 0)
                .WithErrorMessage("Invalid")
                .Build();

            registry.RegisterRule(rule);
            var removed = registry.UnregisterRule(rule.RuleId);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, registry.Count);
            Assert.IsNull(registry.GetRuleById(rule.RuleId));
        }
    }
}
