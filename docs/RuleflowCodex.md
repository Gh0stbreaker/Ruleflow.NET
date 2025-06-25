# Ruleflow Codex

This document describes the recommended protocol for integrating **Ruleflow.NET** into a project. It consolidates configuration tips and common setup patterns into one place so you can get started quickly.

## 1. Add the package

Install the NuGet package in your project:

```bash
dotnet add package Ruleflow.NET
```

## 2. Register services

Use the `AddRuleflow` extension method to register all necessary services in your dependency injection container. This call can automatically register rules and mapping profiles discovered in attributes.

```csharp
services.AddRuleflow<MyModel>(options =>
{
    options.RegisterDefaultValidator = true; // registers IValidator<MyModel>
    options.AutoRegisterAttributeRules = true; // scan assemblies for ValidationRuleAttribute
    options.AutoRegisterMappings = true;       // register MapKeyAttribute mappings
});
```

You can restrict attribute scanning with `AssemblyFilters` or `NamespaceFilters` if needed.

## 3. Define rules

Rules can be defined imperatively using the fluent builders or declaratively with attributes.

### Fluent builders

```csharp
var rule = RuleBuilderFactory
    .CreateUnifiedRuleBuilder<MyModel>()
    .WithValidation((m, ctx) => m.Value > 0)
    .WithErrorMessage("Value must be positive")
    .Build();
```

### Attribute based

```csharp
public static class MyRules
{
    [ValidationRule("PositiveValue", Priority = 1)]
    public static void ValidateValue(MyModel m)
    {
        if (m.Value <= 0)
            throw new ArgumentException("Value must be positive");
    }
}
```

## 4. Create profiles (optional)

`RuleflowProfile<T>` groups mapping and validation rules so they can be registered together.

```csharp
var profile = new RuleflowProfile<MyModel>();
profile.ValidationRules.Add(myRule);
services.AddRuleflow<MyModel>(o => o.RegisterDefaultValidator = true, profile);
```

## 5. Validate

Resolve `IValidator<T>` from the service provider and call `CollectValidationResults` or other helper methods.

```csharp
var validator = serviceProvider.GetRequiredService<IValidator<MyModel>>();
var result = validator.CollectValidationResults(model);
if (!result.IsValid)
    foreach (var error in result.Errors)
        Console.WriteLine(error.Message);
```

This codex should help you adopt Ruleflow.NET quickly and keep configuration consistent across projects.
