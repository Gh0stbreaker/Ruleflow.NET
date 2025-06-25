using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ruleflow.NET.Engine.Models.Rule.Interface;
using Ruleflow.NET.Engine.Registry;
using Ruleflow.NET.Engine.Registry.Interface;
using Ruleflow.NET.Engine.Validation.Core.Context;
using Ruleflow.NET.Engine.Validation.Interfaces;
using Ruleflow.NET.Engine.Validation.Core.Validators;
using Ruleflow.NET.Engine.Validation;
using Ruleflow.NET.Engine.Data.Mapping;
using Ruleflow.NET.Engine.Validation.Core.Base;

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
        public static IServiceCollection AddRuleflow<TInput>(this IServiceCollection services,
            Action<RuleflowOptions<TInput>>? configure = null,
            params RuleflowProfile<TInput>[] profiles)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var options = new RuleflowOptions<TInput>();
            configure?.Invoke(options);

            services.AddSingleton(options);

            var registry = new RuleRegistry<TInput>(options.InitialRules ?? Array.Empty<IRule<TInput>>());

            // Load attribute based validation rules if requested
            if (options.AutoRegisterAttributeRules)
            {
                foreach (var rule in LoadAttributeRules(options))
                    registry.RegisterRule(rule);
            }

            services.AddSingleton<IRuleRegistry<TInput>>(registry);

            var mappingRules = new List<DataMappingRule<TInput>>();
            if (options.AutoRegisterMappings)
                mappingRules.AddRange(AttributeRuleLoader.LoadMappingRules<TInput>());
            foreach (var profile in profiles)
            {
                services.AddSingleton(profile);
                mappingRules.AddRange(profile.MappingRules);
            }
            if (mappingRules.Count > 0)
            {
                services.AddSingleton<IDataAutoMapper<TInput>>(_ => new DataAutoMapper<TInput>(mappingRules));
            }

            // Register ValidationContext singleton so it can be injected where needed
            services.AddSingleton(ValidationContext.Instance);

            // Register a default validator if requested by options
            if (options.RegisterDefaultValidator)
            {
                services.AddSingleton<IValidator<TInput>>(sp =>
                {
                    var reg = sp.GetRequiredService<IRuleRegistry<TInput>>();
                    var validationRules = new List<IValidationRule<TInput>>();
                    foreach (var rule in reg.AllRules)
                    {
                        if (rule is IValidationRule<TInput> vr)
                            validationRules.Add(vr);
                    }
                    foreach (var profile in profiles)
                        validationRules.AddRange(profile.ValidationRules);
                    return new Validator<TInput>(validationRules);
                });
            }

            return services;
        }

        private static IEnumerable<IValidationRule<TInput>> LoadAttributeRules<TInput>(RuleflowOptions<TInput> options)
        {
            var assemblies = ResolveAssemblies(options);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (options.NamespaceFilters != null && !options.NamespaceFilters.Any(ns => type.Namespace != null && type.Namespace.StartsWith(ns)))
                        continue;

                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attr = method.GetCustomAttribute<ValidationRuleAttribute>();
                        if (attr == null) continue;

                        var parameters = method.GetParameters();
                        if (parameters.Length != 1 || parameters[0].ParameterType != typeof(TInput))
                            continue;
                        if (method.ReturnType != typeof(void))
                            continue;

                        var action = (Action<TInput>)Delegate.CreateDelegate(typeof(Action<TInput>), method);
                        var rule = new ActionValidationRule<TInput>(attr.Id, action);
                        rule.SetPriority(attr.Priority);
                        rule.SetSeverity(attr.Severity);
                        yield return rule;
                    }
                }
            }
        }

        private static IEnumerable<Assembly> ResolveAssemblies<TInput>(RuleflowOptions<TInput> options)
        {
            if (options.AssemblyFilters != null && options.AssemblyFilters.Any())
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => options.AssemblyFilters.Contains(a.GetName().Name, StringComparer.OrdinalIgnoreCase));
            }

            return new[] { typeof(TInput).Assembly };
        }
    }
}
