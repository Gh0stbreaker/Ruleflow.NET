using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Models.Rule;
using Ruleflow.NET.Engine.Models.Rule.Group;
using Ruleflow.NET.Engine.Models.Rule.Builder;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class RuleGroupTests
    {
        private class Dummy { }

        [TestMethod]
        public void RuleGroup_AddAndRemoveRule_Works()
        {
            var groupType = new RuleGroupType<Dummy>(1, "CODE", "Group");
            var ruleType = RuleBuilderFactory.CreateRuleType<Dummy>("S", "Single");
            var rule = RuleBuilderFactory.CreateSingleResponsibilityRuleBuilder<Dummy>(ruleType)
                .WithValidation((d, c) => true)
                .WithErrorMessage("err")
                .Build() as Rule<Dummy>;

            var group = new RuleGroup<Dummy>(1, groupType);
            Assert.AreEqual(0, group.Rules.Count);
            group.AddRule(rule);
            Assert.AreEqual(1, group.Rules.Count);
            bool removed = group.RemoveRule(rule);
            Assert.IsTrue(removed);
            Assert.AreEqual(0, group.Rules.Count);
        }

        [TestMethod]
        public void RuleGroupType_StoresProperties()
        {
            var created = DateTimeOffset.UtcNow;
            var type = new RuleGroupType<Dummy>(42, "T", "Name", "Desc", false, created);
            Assert.AreEqual(42, type.Id);
            Assert.AreEqual("T", type.Code);
            Assert.AreEqual("Name", type.Name);
            Assert.AreEqual("Desc", type.Description);
            Assert.IsFalse(type.IsEnabled);
            Assert.AreEqual(created.ToUniversalTime(), type.CreatedAt);
        }
    }
}
