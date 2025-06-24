using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Validation.Core.Validators;
using System;
using System.Collections.Generic;

namespace Ruleflow.NET.Tests
{
    [TestClass]
    public class BatchValidatorTests
    {
        private class Item
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
        }

        [TestMethod]
        public void BatchValidator_MixedInputs_AggregatesErrors()
        {
            // Arrange
            var quantityRule = RuleflowExtensions.CreateRule<Item>()
                .WithAction(i =>
                {
                    if (i.Quantity <= 0)
                        throw new ArgumentException("Quantity must be positive");
                })
                .Build();

            var validator = new BatchValidator<Item>(new[] { quantityRule });

            var valid = new Item { Name = "Valid", Quantity = 1 };
            var invalid1 = new Item { Name = "Bad1", Quantity = 0 };
            var invalid2 = new Item { Name = "Bad2", Quantity = -5 };
            var batch = new List<Item> { valid, invalid1, invalid2 };

            // Act
            var result = validator.CollectValidationResults(batch);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
        }

        [TestMethod]
        public void BatchValidator_ValidateAndProcess_ExecutesOnSuccess()
        {
            // Arrange
            var quantityRule = RuleflowExtensions.CreateRule<Item>()
                .WithAction(i =>
                {
                    if (i.Quantity <= 0)
                        throw new ArgumentException("Quantity must be positive");
                })
                .Build();

            var validator = new BatchValidator<Item>(new[] { quantityRule });

            var batch = new List<Item>
            {
                new Item { Name = "Item1", Quantity = 2 },
                new Item { Name = "Item2", Quantity = 3 }
            };

            bool processed = false;

            // Act
            var result = validator.ValidateAndProcess(batch, _ => processed = true);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(processed);
        }
    }
}
