using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Events;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Core.Validators;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class EventTriggerRuleTests
    {
        [TestMethod]
        public void EventTriggerRule_InvokesRegisteredEvent()
        {
            // Arrange
            bool triggered = false;
            EventHub.Clear();
            EventHub.Register("TestEvent", () => triggered = true);
            var rule = RuleflowExtensions.CreateEventRule<object>("TestEvent").Build();
            var validator = new Validator<object>(new[] { rule });

            // Act
            var result = validator.CollectValidationResults(new object());

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(triggered);
        }
    }
}
