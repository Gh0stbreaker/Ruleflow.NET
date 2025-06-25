using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Ruleflow.NET.Engine.Data.Enums;
using Ruleflow.NET.Engine.Data.Mapping;
using Ruleflow.NET.Engine.Extensions;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Core.Validators;
using Ruleflow.NET.Engine.Validation.Enums;
using Ruleflow.NET.Extensions;
using Ruleflow.NET.Engine.Registry.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class AttributeIntegrationTests
    {
        private class Person
        {
            [MapKey("name", DataType.String, Required = true)]
            public string Name { get; set; } = string.Empty;

            [MapKey("age", DataType.Int32, Required = true)]
            public int Age { get; set; }
        }

        private static class PersonRules
        {
            [ValidationRule("NameRequired", Severity = ValidationSeverity.Error)]
            public static void NameRequired(Person p)
            {
                if (string.IsNullOrWhiteSpace(p.Name))
                    throw new ArgumentException("Name required");
            }
        }

        [TestMethod]
        public void DataAutoMapper_FromAttributes_Works()
        {
            var mapper = DataAutoMapper<Person>.FromAttributes();
            var context = new DataContext();
            var data = new Dictionary<string, string> { { "name", "John" }, { "age", "42" } };
            var person = mapper.MapToObject(data, context);

            Assert.AreEqual("John", person.Name);
            Assert.AreEqual(42, person.Age);
            Assert.AreEqual(2, context.Values.Count);

            var back = mapper.MapToData(person, new DataContext());
            Assert.AreEqual("John", back["name"].Value);
            Assert.AreEqual(42, back["age"].Value);
        }

        [TestMethod]
        public void AttributeRules_CombineWithFluent_Works()
        {
            var attributeRules = AttributeRuleLoader.LoadValidationRules<Person>();
            var ageRule = Ruleflow.NET.Engine.Validation.RuleflowExtensions.CreateRule<Person>()
                .WithAction(p => { if (p.Age <= 0) throw new ArgumentException("Age"); })
                .Build();

            var validator = new Validator<Person>(attributeRules.Concat(new[] { ageRule }));

            var invalid = new Person { Name = "", Age = -1 };
            Assert.IsFalse(validator.IsValid(invalid));

            var valid = new Person { Name = "Jane", Age = 20 };
            Assert.IsTrue(validator.IsValid(valid));
        }

        [TestMethod]
        public void AddRuleflow_AutoRegistersRulesAndMapper()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>(o =>
            {
                o.AutoRegisterAttributeRules = true;
                o.AutoRegisterMappings = true;
                o.AssemblyFilters = new[] { typeof(PersonRules).Assembly.GetName().Name };
                o.NamespaceFilters = new[] { typeof(AttributeIntegrationTests).Namespace! };
            });

            var provider = services.BuildServiceProvider();

            var registry = provider.GetRequiredService<IRuleRegistry<Person>>();
            Assert.AreEqual(1, registry.Count);

            var mapper = provider.GetRequiredService<IDataAutoMapper<Person>>();
            var context = new DataContext();
            var data = new Dictionary<string, string> { { "name", "Bob" }, { "age", "30" } };
            var person = mapper.MapToObject(data, context);
            Assert.AreEqual("Bob", person.Name);
            Assert.AreEqual(30, person.Age);
        }

        [TestMethod]
        public void AddRuleflow_NamespaceFilter_ExcludesRules()
        {
            var services = new ServiceCollection();
            services.AddRuleflow<Person>(o =>
            {
                o.AutoRegisterAttributeRules = true;
                o.AutoRegisterMappings = true;
                o.NamespaceFilters = new[] { "Other.Namespace" };
            });

            var provider = services.BuildServiceProvider();

            var registry = provider.GetRequiredService<IRuleRegistry<Person>>();
            Assert.AreEqual(0, registry.Count);

            // mapper still registered
            Assert.IsNotNull(provider.GetRequiredService<IDataAutoMapper<Person>>());
        }
    }
}
