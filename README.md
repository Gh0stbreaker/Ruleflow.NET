<div align="center">
  <img src="Ruleflow.NET/Engine/Images/Ruleflow.NET.png" alt="Ruleflow.NET logo" />
</div>

# Ruleflow.NET

Ruleflow.NET is a flexible rule and validation framework for .NET 8. It lets you compose complex business logic with a fluent API while keeping application code clean. The engine supports dependency-aware execution, data mapping utilities and a lightweight event system.

## ✨ Key Features

- **Intuitive Fluent API** - Create complex validation rules with a natural, readable syntax
- **Conditional Logic** - Build sophisticated rule flows with if/then/else and switch expressions
- **Rule Dependencies** - Define rules that depend on the results of other rules
- **Prioritized Execution** - Control the order of rule evaluation with priority settings
- **Comprehensive Results** - Get detailed validation results with configurable severity levels
- **Dependency Awareness** - Built-in dependency graph validation to prevent circular references
- **Clean Separation** - Keep your business logic separate from your application code
- **Intelligent Rule References** - Use lightweight references that resolve rules from a registry when needed
- **Flexible Data Mapping** - Convert dictionaries to objects and back using the built-in DataAutoMapper
- **Batch Validation** - Validate collections of inputs with `BatchValidator`
- **Composite Validators** - Merge results from multiple validators using `CompositeValidator`
- **Shared Validation Context** - Pass data and rule results between rules via `ValidationContext`
- **Event and Action Hooks** - Trigger custom actions or events from validation rules

## 🏗️ Architecture

Ruleflow.NET is built around a set of core components:

- **`IValidationRule<T>`** - Base interface for validation rules
- **`IValidator<T>`** - Interface for validators that execute rules
- **`ValidationContext`** - Shares data and rule results during validation
- **`DependencyAwareValidator<T>`** - Handles rule dependencies and priorities
- **`DataAutoMapper<T>`** - Maps dictionaries to typed objects
- **`RuleReference<T>`** - Lightweight references that resolve rules from a registry

## 🌐 Use Cases

- **Form Validation** - Validate user input with complex business rules
- **API Request Validation** - Ensure incoming requests meet your requirements
- **Business Rule Processing** - Execute business logic in a structured, maintainable way
- **Workflow Validation** - Verify that each step in a workflow can proceed
- **Data Integrity Checks** - Keep your data consistent with your domain rules

## Why Ruleflow.NET?

- Clear separation of rules from application code
- Reusable rules and validators with minimal boilerplate
- Works with dependency injection for easy integration
- Supports attribute-based configuration and fluent builders

## Getting started

### Installation

```bash
# Package will be available on NuGet
dotnet add package Ruleflow.NET
```

### Basic usage

```csharp
var ageRule = RuleflowExtensions.CreateRule<Person>()
    .WithAction(p =>
    {
        if (p.Age < 18)
            throw new ArgumentException("Person must be an adult");
    })
    .WithMessage("Age validation failed")
    .Build();

var validator = new Validator<Person>(new[] { ageRule });
var result = validator.CollectValidationResults(new Person { Name = "John", Age = 17 });

foreach (var error in result.Errors)
    Console.WriteLine($"{error.Severity}: {error.Message}");
```

### Real-world example

```csharp
public class SignUpModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

var usernameRule = RuleflowExtensions.CreateRule<SignUpModel>()
    .WithId("UsernameValidation")
    .WithAction(m =>
    {
        if (string.IsNullOrWhiteSpace(m.Username))
            throw new ArgumentException("Username is required");
        if (m.Username.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters");
    })
    .Build();

var passwordRule = RuleflowExtensions.CreateRule<SignUpModel>()
    .WithId("PasswordValidation")
    .WithAction(m =>
    {
        if (string.IsNullOrWhiteSpace(m.Password) || m.Password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters");
    })
    .Build();

var emailRule = RuleflowExtensions.CreateDependentRule<SignUpModel>("EmailValidation")
    .DependsOn("UsernameValidation")
    .WithDependencyType(DependencyType.RequiresAllSuccess)
    .WithAction(m =>
    {
        if (string.IsNullOrWhiteSpace(m.Email) || !m.Email.Contains("@"))
            throw new ArgumentException("Valid email is required");
    })
    .Build();

var validator = new DependencyAwareValidator<SignUpModel>(
    new[] { usernameRule, passwordRule, emailRule });

var signUp = new SignUpModel
{
    Username = "jo",
    Email = "invalid",
    Password = "pw"
};

var result2 = validator.CollectValidationResults(signUp);

foreach (var error in result2.Errors)
    Console.WriteLine(error.Message);
```

### Dependency injection

```csharp
using Microsoft.Extensions.DependencyInjection;

var profile = AttributeRuleLoader.LoadProfile<Person>();

var services = new ServiceCollection();
services.AddRuleflow<Person>(options => options.InitialRules = new[] { ageRule }, profile);
var provider = services.BuildServiceProvider();
```

### Implementing in your project

1. Install the NuGet package
2. Define your validation rules using the fluent builders or attributes
3. Register Ruleflow in your dependency injection container
4. Create a validator and call `CollectValidationResults`

Additional examples can be found in the unit tests inside `Ruleflow.NET.Tests`.

## Roadmap

- better & simplistic configuration in project

## Building and tests

Run the tests using the .NET SDK:

```bash
dotnet test
```

## Contributing

Contributions and feedback are welcome. Feel free to open issues or submit pull requests.

## License

This project is licensed under the [Apache License 2.0](LICENSE.txt).
