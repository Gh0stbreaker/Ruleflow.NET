using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Registry;
using Ruleflow.NET.Engine.Registry.Interface;
using Ruleflow.NET.Engine.Validation.Core.Context;
using Ruleflow.NET.Engine.Validation.Interfaces;
using Ruleflow.NET.Engine.Validation.Core.Validators;

namespace Ruleflow.NET.Extensions
{
    /// <summary>
    /// Extension methods for registering Ruleflow services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds core Ruleflow services to the DI container.
        /// </summary>
        /// <typeparam name="TInput">Type of objects being validated.</typeparam>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="configure">Optional configuration for initial rules.</param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection AddRuleflow<TInput>(this IServiceCollection services, Action<RuleflowOptions<TInput>>? configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var options = new RuleflowOptions<TInput>();
            configure?.Invoke(options);

            services.AddSingleton(options);

            services.AddSingleton<IRuleRegistry<TInput>>(sp => new RuleRegistry<TInput>(options.InitialRules ?? Array.Empty<IRule<TInput>>()));

            // Register ValidationContext singleton so it can be injected where needed
            services.AddSingleton(ValidationContext.Instance);

            // Register a default validator if requested by options
            if (options.RegisterDefaultValidator)
            {
                services.AddSingleton<IValidator<TInput>>(sp =>
                {
                    var registry = sp.GetRequiredService<IRuleRegistry<TInput>>();
                    var validationRules = new List<IValidationRule<TInput>>();
                    foreach (var rule in registry.AllRules)
                    {
                        if (rule is IValidationRule<TInput> vr)
                            validationRules.Add(vr);
                    }
                    return new Validator<TInput>(validationRules);
                });
            }

            return services;
        }
    }
}
