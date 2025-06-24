using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Extensions;
using Ruleflow.NET.Engine.Registry.Interface;
using Ruleflow.NET.Engine.Validation.Interfaces;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        private class Person
        {
            public int Age { get; set; }
        }

        [TestMethod]
        public void AddRuleflow_RegistersRegistryAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>();
            var provider = services.BuildServiceProvider();

            var reg1 = provider.GetRequiredService<IRuleRegistry<Person>>();
            var reg2 = provider.GetRequiredService<IRuleRegistry<Person>>();

            Assert.IsNotNull(reg1);
            Assert.AreSame(reg1, reg2);
        }

        [TestMethod]
        public void AddRuleflow_WithInitialRules_RegistersThem()
        {
            var rule = Ruleflow.NET.Engine.Models.Rule.Builder.RuleBuilderFactory
                .CreateSingleResponsibilityRuleBuilder<Person>()
                .WithValidation((p, ctx) => p.Age >= 18)
                .WithErrorMessage("Age must be at least 18")
                .Build();

            var services = new ServiceCollection();
            services.AddRuleflow<Person>(o => o.InitialRules = new[] { rule });
            var provider = services.BuildServiceProvider();

            var registry = provider.GetRequiredService<IRuleRegistry<Person>>();
            Assert.AreEqual(1, registry.Count);
        }

        [TestMethod]
        public void AddRuleflow_RegistersValidationContext()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>();
            var provider = services.BuildServiceProvider();

            var context1 = provider.GetRequiredService<Ruleflow.NET.Engine.Validation.Core.Context.ValidationContext>();
            var context2 = provider.GetRequiredService<Ruleflow.NET.Engine.Validation.Core.Context.ValidationContext>();
            Assert.AreSame(context1, context2);
        }
    }
}
