# Application Layer Organization

## Purpose

This document describes the responsibilities, internal organization, and design principles of the **Application** layer.

The Application layer coordinates use cases by orchestrating domain objects, repositories, and domain services. It contains no business rules; those always remain inside the Domain layer.

---

## Responsibilities

The Application layer is responsible for:

- Executing application use cases.
- Coordinating Aggregate Roots.
- Loading and persisting domain objects through repository abstractions.
- Invoking Domain Services when required.
- Returning application-specific results.

The Application layer is **not** responsible for:

- Business rules.
- Aggregate consistency.
- Persistence implementation.
- HTTP concerns.
- Infrastructure concerns.
- User interface concerns.

---

## Layer Dependencies

The Application layer depends only on the Domain layer.

```text
                +--------------------+
                |        API         |
                +--------------------+
                          │
                          ▼
                +--------------------+
                |    Application     |
                +--------------------+
                          │
                          ▼
                +--------------------+
                |       Domain       |
                +--------------------+

                Infrastructure
                       ▲
                       │
          Implements repository abstractions
```

Repository implementations belong to **Infrastructure**, while repository abstractions belong to **Application**.

---

## Folder Organization

The Application layer is organized by **feature** rather than by technical artifact.

Example:

```text
Beridian.Application
│
├── FinancialPeriods
│   ├── CreateFinancialPeriod
│   │   ├── CreateFinancialPeriodCommand.cs
│   │   ├── CreateFinancialPeriodHandler.cs
│   │   └── CreateFinancialPeriodResult.cs
│   │
│   ├── GenerateNextFinancialPeriod
│   ├── CloseFinancialPeriod
│   └── ...
│
├── Expenses
├── Incomes
└── Investments
```

This organization keeps all artifacts belonging to the same use case together, improving discoverability and maintainability.

---

## Use Case Structure

Each use case follows the same internal structure.

```text
UseCase
├── Command
├── Handler
└── Result
```

### Command

Represents the intention of the use case.

A Command contains all information required to execute a business operation.

Example:

```csharp
public sealed record CloseFinancialPeriodCommand(
    Guid FinancialPeriodId);
```

---

### Handler

The Handler coordinates the execution of the use case.

Its responsibilities are limited to:

- loading aggregates;
- invoking domain behavior;
- invoking domain services when required;
- persisting changes;
- returning the application result.

Handlers should remain thin and contain no business rules.

Typical execution flow:

```text
Load Aggregate
        │
        ▼
Invoke Domain Behavior
        │
        ▼
Persist Changes
        │
        ▼
Return Result
```

---

### Result

Represents the outcome of the use case.

Results should expose only the information required by the caller.

Example:

```csharp
public sealed record CloseFinancialPeriodResult(
    Guid FinancialPeriodId);
```

---

## Repository Abstractions

Repository interfaces belong to the Application layer.

Example:

```csharp
public interface IFinancialPeriodRepository
{
    Task<FinancialPeriod?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        FinancialPeriod financialPeriod,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        FinancialPeriod financialPeriod,
        CancellationToken cancellationToken = default);
}
```

Application depends only on these abstractions.

Concrete implementations belong to the Infrastructure layer.

---

## Dependency Injection

Each project registers its own services through a dedicated `DependencyInjection` class.

Typical registrations include:

### Application

- Use Case Handlers
- Domain Services
- Future validators or pipeline behaviors

### Infrastructure

- Repository implementations
- Database context
- External services

The API project acts as the Composition Root.

```text
Program.cs
        │
        ├── AddApplication()
        └── AddInfrastructure()
```

This keeps dependencies flowing inward while allowing Infrastructure to provide concrete implementations.

---

## Command Execution Flow

The complete execution flow of a typical use case is illustrated below.

```text
HTTP Request
      │
      ▼
Controller
      │
      ▼
Command
      │
      ▼
Handler
      │
      ▼
Repository
      │
      ▼
Aggregate Root
      │
      ▼
Domain Services (optional)
      │
      ▼
Repository
      │
      ▼
Result
      │
      ▼
HTTP Response
```

The Handler coordinates the execution.

The Domain decides.

---

## Design Principles

The Application layer follows these principles:

- Organize code by feature.
- One Handler per use case.
- One Command per use case.
- One Result per use case.
- Keep Handlers thin.
- Never duplicate business rules.
- Depend on abstractions, never implementations.
- Coordinate the Domain rather than replacing it.

---

## Domain Event Dispatching

Application is responsible for coordinating the dispatch of Domain Events after the related aggregate changes have been persisted.

Typical flow:

```text
Execute Domain Operation
        ↓
Persist Aggregate
        ↓
Dispatch Domain Events
        ↓
Clear Domain Events

---

## Related Documentation

- `clean-architecture.md`
- `domain-model/`
- `../adr/ADR-001-adopt-clean-architecture.md`
- `../development/coding-conventions.md`
- `../development/testing-conventions.md`