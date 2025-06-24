<div align="center">
  <img src="Ruleflow.NET/Engine/Images/Ruleflow.NET.png" alt="Ruleflow.NET Logo">
</div>

# Ruleflow.NET

## English

### 🌟 Overview

Ruleflow.NET is a flexible, high-performance business rules and validation framework for .NET applications. Built with modern C# features and a fluent API design, Ruleflow.NET helps you create, manage, and execute complex business logic and validation rules with minimal code and maximum readability.

> ⚠️ **Note:** Ruleflow.NET is currently under active development. While core functionality is stable, some features may change before the final release.

### ✨ Key Features

- **Intuitive Fluent API** - Create complex validation rules with a natural, readable syntax
- **Conditional Logic** - Build sophisticated rule flows with if/then/else and switch expressions
- **Rule Dependencies** - Define rules that depend on the results of other rules
- **Prioritized Execution** - Control the order of rule evaluation with priority settings
- **Comprehensive Results** - Get detailed validation results with configurable severity levels
- **Dependency Awareness** - Built-in dependency graph validation to prevent circular references
- **Clean Separation** - Keep your business logic separate from your application code

### 🚀 Getting Started

#### Installation

```bash
# Coming soon to NuGet!
dotnet add package Ruleflow.NET
```

#### Basic Usage

```csharp
// Create a simple validation rule
var ageRule = RuleflowExtensions.CreateRule<Person>()
    .WithAction(p => {
        if (p.Age < 18)
            throw new ArgumentException("Person must be at least 18 years old");
    })
    .WithMessage("Age validation failed")
    .WithSeverity(ValidationSeverity.Error)
    .Build();

// Create a validator with our rule
var validator = new Validator<Person>(new[] { ageRule });

// Validate a person
var person = new Person { Name = "John", Age = 17 };
var result = validator.CollectValidationResults(person);

// Check the result
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Severity}: {error.Message}");
    }
}
```

#### Advanced Validation with Conditional Logic

```csharp
// Create a conditional validation rule
var drivingRule = RuleflowExtensions
    .CreateConditionalRule<Person>(p => p.HasDrivingLicense)
    .Then(builder => builder
        .WithAction(p => {
            if (p.Age < 16)
                throw new ArgumentException("Driving license holders must be at least 16");
        })
        .WithMessage("Invalid driving license")
    )
    .Else(builder => builder
        .WithAction(p => {
            if (p.Age < 13)
                throw new ArgumentException("Non-drivers must be at least 13");
        })
        .WithMessage("Age validation failed")
    )
    .Build();
```

### 🔧 Advanced Features

#### Dependent Rules

```csharp
// Create rules with dependencies
var primaryRule = RuleflowExtensions.CreateRule<Order>()
    .WithId("PrimaryCheck")
    .WithAction(o => {
        if (o.Amount <= 0)
            throw new ArgumentException("Order amount must be positive");
    })
    .Build();

var dependentRule = RuleflowExtensions.CreateDependentRule<Order>("SecondaryCheck")
    .DependsOn("PrimaryCheck")
    .WithDependencyType(DependencyType.RequiresAllSuccess)
    .WithAction(o => {
        if (o.Items.Count == 0)
            throw new ArgumentException("Order must have at least one item");
    })
    .Build();
```

#### Switch Pattern Rules

```csharp
// Create a switch-based rule
var statusRule = RuleflowExtensions
    .CreateSwitchRule<Order, OrderStatus>(o => o.Status)
    .Case(OrderStatus.Draft, builder => builder
        .WithAction(o => {
            if (o.DraftDate == null)
                throw new ArgumentException("Draft orders must have a draft date");
        })
    )
    .Case(OrderStatus.Submitted, builder => builder
        .WithAction(o => {
            if (o.SubmissionDate == null)
                throw new ArgumentException("Submitted orders must have a submission date");
        })
    )
    .Default(builder => builder
        .WithAction(o => {
            if (o.LastModified == null)
                throw new ArgumentException("All orders must have a last modified date");
        })
    )
    .Build();
```

### 🏗️ Architecture

Ruleflow.NET is designed around a set of core interfaces and components:

- **`IValidationRule<T>`** - Base interface for all validation rules
- **`IValidator<T>`** - Interface for validators that can validate objects of type T
- **`IValidationResult`** - Contains the results of a validation operation
- **`ValidationRuleBuilder<T>`** - Fluent API for constructing validation rules
- **`DependencyAwareValidator<T>`** - Validator that supports rule dependencies
- **`ValidationContext`** - Context for validation operations, including results of rule evaluations

### 🌐 Use Cases

- **Form Validation** - Validate user input with complex business rules
- **API Request Validation** - Ensure incoming requests meet your requirements
- **Business Rule Processing** - Execute business logic in a structured, maintainable way
- **Workflow Validation** - Verify that each step in a workflow can proceed
- **Data Integrity Checks** - Ensure your data meets your business constraints

### 📋 Roadmap
- [ ] Coming soon!

### 📚 Documentation

Comprehensive documentation is in progress and will be available soon.

### 📝 Code Comments

The source code now contains detailed XML comments in both Czech and English. These comments explain the purpose of each class and method to help new contributors understand how the framework works.

### 🤝 Contributing

Contributions are welcome! Feel free to submit issues or pull requests to help improve Ruleflow.NET.

### 📄 License

Ruleflow.NET is licensed under the [Apache License 2.0](LICENSE.txt).

---

Made with ❤️

## Česky

### 🌟 Přehled

Ruleflow.NET je flexibilní a výkonný rámec pro obchodní pravidla a validaci v .NET aplikacích. Díky moderním funkcím jazyka C# a přehlednému rozhraní Fluent API vám umožní vytvářet, spravovat a vykonávat složitou obchodní logiku a validační pravidla s minimem kódu a maximální čitelností.

> ⚠️ **Poznámka:** Ruleflow.NET je momentálně aktivně vyvíjen. Ačkoli je základní funkčnost stabilní, některé vlastnosti se mohou před finálním vydáním změnit.

### ✨ Hlavní funkce

- **Přehledné Fluent API** – Tvořte komplexní validační pravidla přirozenou a čitelnou syntaxí
- **Podmíněná logika** – Stavte sofistikované toky pravidel pomocí konstrukcí if/then/else a switch
- **Závislosti pravidel** – Definujte pravidla, která se odvíjejí od výsledků jiných pravidel
- **Prioritní spouštění** – Ovládejte pořadí vyhodnocování pravidel nastavením priorit
- **Podrobné výsledky** – Získejte detailní validační výsledky s konfigurovatelnou závažností
- **Vědomí závislostí** – Vestavěná kontrola grafu závislostí brání tvorbě cyklických odkazů
- **Čisté oddělení** – Udržujte obchodní logiku oddělenou od aplikačního kódu

### 🚀 Začínáme

#### Instalace

```bash
# Již brzy na NuGet!
dotnet add package Ruleflow.NET
```

#### Základní použití

```csharp
// Vytvoření jednoduchého validačního pravidla
var ageRule = RuleflowExtensions.CreateRule<Person>()
    .WithAction(p => {
        if (p.Age < 18)
            throw new ArgumentException("Osoba musí být starší než 18 let");
    })
    .WithMessage("Chyba při ověření věku")
    .WithSeverity(ValidationSeverity.Error)
    .Build();

// Vytvoření validátoru s naším pravidlem
var validator = new Validator<Person>(new[] { ageRule });

// Validace osoby
var person = new Person { Name = "John", Age = 17 };
var result = validator.CollectValidationResults(person);

// Kontrola výsledku
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Severity}: {error.Message}");
    }
}
```

#### Pokročilá validace s podmínkami

```csharp
// Vytvoření podmíněného validačního pravidla
var drivingRule = RuleflowExtensions
    .CreateConditionalRule<Person>(p => p.HasDrivingLicense)
    .Then(builder => builder
        .WithAction(p => {
            if (p.Age < 16)
                throw new ArgumentException("Držitelé řidičského průkazu musí mít alespoň 16 let");
        })
        .WithMessage("Neplatný řidičský průkaz")
    )
    .Else(builder => builder
        .WithAction(p => {
            if (p.Age < 13)
                throw new ArgumentException("Nedržitelé průkazu musí mít alespoň 13 let");
        })
        .WithMessage("Chyba při ověření věku")
    )
    .Build();
```

### 🔧 Pokročilé funkce

#### Závislá pravidla

```csharp
// Vytvoření pravidel se závislostmi
var primaryRule = RuleflowExtensions.CreateRule<Order>()
    .WithId("PrimaryCheck")
    .WithAction(o => {
        if (o.Amount <= 0)
            throw new ArgumentException("Hodnota objednávky musí být kladná");
    })
    .Build();

var dependentRule = RuleflowExtensions.CreateDependentRule<Order>("SecondaryCheck")
    .DependsOn("PrimaryCheck")
    .WithDependencyType(DependencyType.RequiresAllSuccess)
    .WithAction(o => {
        if (o.Items.Count == 0)
            throw new ArgumentException("Objednávka musí obsahovat alespoň jednu položku");
    })
    .Build();
```

#### Pravidla typu switch

```csharp
// Vytvoření pravidla založeného na konstrukci switch
var statusRule = RuleflowExtensions
    .CreateSwitchRule<Order, OrderStatus>(o => o.Status)
    .Case(OrderStatus.Draft, builder => builder
        .WithAction(o => {
            if (o.DraftDate == null)
                throw new ArgumentException("Návrhy objednávek musí mít datum návrhu");
        })
    )
    .Case(OrderStatus.Submitted, builder => builder
        .WithAction(o => {
            if (o.SubmissionDate == null)
                throw new ArgumentException("Odeslané objednávky musí mít datum odeslání");
        })
    )
    .Default(builder => builder
        .WithAction(o => {
            if (o.LastModified == null)
                throw new ArgumentException("Všechny objednávky musí mít datum poslední úpravy");
        })
    )
    .Build();
```

### 🏗️ Architektura

Ruleflow.NET je postaven na sadě základních rozhraní a komponent:

- **`IValidationRule<T>`** – Základní rozhraní pro všechna validační pravidla
- **`IValidator<T>`** – Rozhraní pro validátory, které mohou ověřovat objekty typu T
- **`IValidationResult`** – Obsahuje výsledky validační operace
- **`ValidationRuleBuilder<T>`** – Fluent API pro tvorbu validačních pravidel
- **`DependencyAwareValidator<T>`** – Validátor podporující závislosti mezi pravidly
- **`ValidationContext`** – Kontext validačních operací včetně výsledků vyhodnocení pravidel

### 🌐 Příklady použití

- **Ověřování formulářů** – Validace vstupů od uživatelů pomocí komplexních pravidel
- **Ověřování API požadavků** – Zajištění, že příchozí požadavky splňují vaše podmínky
- **Zpracování obchodních pravidel** – Spouštění obchodní logiky strukturovaným a udržitelným způsobem
- **Validace pracovních postupů** – Ověření, že každý krok ve workflow může pokračovat
- **Kontrola integrity dat** – Zajistěte, že vaše data splňují obchodní omezení

### 📋 Plán vývoje
- [ ] Již brzy!

### 📚 Dokumentace

Kompletní dokumentace je ve vývoji a bude brzy dostupná.

### 📝 Komentáře v kódu

Zdrojový kód nyní obsahuje podrobné XML komentáře v češtině i angličtině. Tyto komentáře vysvětlují účel každé třídy a metody a pomáhají novým přispěvatelům pochopit fungování rámce.

### 🤝 Přispívání

Přispěvatelé jsou vítáni! Neváhejte zasílat issue nebo pull request pro zlepšení Ruleflow.NET.

### 📄 Licence

Ruleflow.NET je licencován pod [Apache License 2.0](LICENSE.txt).

---

Vytvořeno s ❤️

