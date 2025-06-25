using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Mapping;
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

        internal class MappedPerson
        {
            [MapKey("name", DataType.String, Required = true)]
            public string Name { get; set; } = string.Empty;

            [MapKey("age", DataType.Int32, Required = true)]
            public int Age { get; set; }
        }

        private static class MappedPersonRules
        {
            [ValidationRule("NameRequired", Severity = ValidationSeverity.Error)]
            public static void NameRequired(MappedPerson p)
            {
                if (string.IsNullOrWhiteSpace(p.Name))
                    throw new ArgumentException("Name required");
            }
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
                .CreateUnifiedRuleBuilder<Person>()
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

        [TestMethod]
        public void AddRuleflow_RegisterDefaultValidator_AddsValidator()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>(o => o.RegisterDefaultValidator = true);
            var provider = services.BuildServiceProvider();

            var validator1 = provider.GetRequiredService<IValidator<Person>>();
            var validator2 = provider.GetRequiredService<IValidator<Person>>();

            Assert.IsNotNull(validator1);
            Assert.AreSame(validator1, validator2);
        }

        [TestMethod]
        public void AddRuleflow_WithoutRegisterDefaultValidator_NoValidatorRegistered()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>();
            var provider = services.BuildServiceProvider();

            var validator = provider.GetService<IValidator<Person>>();
            Assert.IsNull(validator);
        }

        [TestMethod]
        public void AddRuleflow_AutoRegisterAttributeRules_LoadsRulesAndMapper()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<MappedPerson>(o =>
            {
                o.AutoRegisterAttributeRules = true;
                o.AssembliesToScan = new[] { typeof(ServiceCollectionExtensionsTests).Assembly };
            });

            var provider = services.BuildServiceProvider();

            var registry = provider.GetRequiredService<IRuleRegistry<MappedPerson>>();
            Assert.IsNotNull(registry.GetRuleById("NameRequired"));

            var mapper = provider.GetRequiredService<IDataAutoMapper<MappedPerson>>();
            var person = mapper.MapToObject(new Dictionary<string, string> { { "name", "John" }, { "age", "50" } }, new DataContext());
            Assert.AreEqual("John", person.Name);
            Assert.AreEqual(50, person.Age);
        }

        [TestMethod]
        public void AddRuleflow_NamespaceFilter_Works()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<MappedPerson>(o =>
            {
                o.AutoRegisterAttributeRules = true;
                o.AssembliesToScan = new[] { typeof(ServiceCollectionExtensionsTests).Assembly };
                o.NamespaceFilters = new[] { typeof(MappedPersonRules).Namespace! };
            });

            var provider = services.BuildServiceProvider();

            var registry = provider.GetRequiredService<IRuleRegistry<MappedPerson>>();
            Assert.IsNotNull(registry.GetRuleById("NameRequired"));
            Assert.IsNull(registry.GetRuleById("OtherRule"));
        }
    }

    namespace External
    {
        internal static class OtherRules
        {
            [ValidationRule("OtherRule")]
            public static void Other(ServiceCollectionExtensionsTests.MappedPerson p) { }
        }
    }
}
