# Coding Conventions

## Purpose
Define common coding conventions for the Beridian codebase to improve readability,
consistency, maintainability, and code review quality.

These conventions complement the automated rules defined in `.editorconfig`.

---

## Naming

### Types and Members

Use `PascalCase` for:

- classes;
- records;
- interfaces;
- enums;
- methods;
- properties.

```csharp
public sealed class FinancialPeriodGenerator
{
    public FinancialPeriod Generate(...)
}
```

Use camelCase for:

- parameters;
- local variables.

```csharp
var financialPeriod = ...
```

Use _camelCase for private fields.
```csharp
private readonly IFinancialPeriodRepository _repository;
```

Interfaces use the I prefix.
```csharp
IFinancialPeriodRepository
```
---

### Method and Constructor Calls

Keep short calls with simple arguments on a single line.
```csharp
var period = Period.Create(2026, 8);

var money = Money.Create(54_000m, Currency.Clp);
```

When a call contains several arguments or nested expressions, place each argument
on a separate line.
```csharp
var command = new AddFixedTermExpenseCommand(
    financialPeriod.Id,
    "Celular 8de12",
    Money.Create(54_000m, Currency.Clp),
    8,
    12);
```
The goal is readability rather than enforcing a fixed line length-rule.

---

### Logical Grouping

Use blank lines to separate logical groups of statements.
```csharp
var repository = new FakeFinancialPeriodRepository();

var handler = new AddFixedTermExpenseHandler(repository);

var command = new AddFixedTermExpenseCommand(
    Guid.NewGuid(),
    "Celular 8de12",
    Money.Create(54_000m, Currency.Clp),
    8,
    12);
```

Blank lines should communicate grouping, not simply separate every statement.
---

var ### Usage

Use var when the type is obvious from the right-hand side.
```csharp
var repository = new FakeFinancialPeriodRepository();
```
Prefer an explicit type when it improves understanding or communicates an abstraction.

```csharp
Expense expense = RecurringExpense.Create(...);
```

---
### Comments

Comments should explain intent, constraints, or non-obvious decisions.
Avoid comments that merely repeat the code.
Prefer expressive code over explanatory comments whenever possible.

### File Organization

Organize production code primarily by domain concept or feature rather than by
technical artifact.

Examples:
```text
Beridian.Domain
├── Expenses
├── FinancialPeriods
├── Incomes
├── Investments
└── Services
Beridian.Application
├── Expenses
├── FinancialPeriods
├── Incomes
└── Investments
```
---

### Automated Formatting

Formatting rules that can be enforced automatically should be configured in
.editorconfig.

The Markdown document explains intent and conventions that cannot be fully
captured by tooling.

