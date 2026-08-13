# Domain Layer

## Purpose

This document defines the architectural role, responsibilities, dependencies, and design principles of the **Domain** layer in Beridian.

The Domain layer contains the business model and protects its rules independently from application workflows, persistence mechanisms, transport protocols, and infrastructure concerns.

The detailed business model is documented separately under `domain-model/`.

---

## Responsibilities

The Domain layer is responsible for:

* Representing business concepts and behavior.
* Enforcing business rules and invariants.
* Protecting Aggregate Root consistency boundaries.
* Managing domain lifecycle transitions.
* Encapsulating domain state and behavior.
* Representing domain concepts through Entities and Value Objects.
* Coordinating domain behavior through Domain Services when required.
* Recording relevant Domain Events.
* Performing calculations derived from domain state.

The Domain layer is the authoritative source of business behavior.

---

## Non-Responsibilities

The Domain layer is **not** responsible for:

* Application use case orchestration.
* HTTP requests or responses.
* Persistence implementation.
* Database access.
* Entity Framework Core configuration.
* External service integrations.
* Dependency injection configuration.
* User interface concerns.
* Transport or serialization concerns.

These responsibilities belong to outer architectural layers.

---

## Dependencies

The Domain layer must not depend on any other Beridian project.

```text
Beridian.Domain
      │
      └── No project dependencies
```

Outer layers may depend on Domain:

```text
API
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure ──────► Domain
```

The dependency direction always points inward.

Domain must never reference:

```text
Beridian.Application
Beridian.Infrastructure
Beridian.Api
```

This preserves the independence of the business model from technical implementation details.

---

## Internal Organization

The Domain layer is organized primarily by **business concept** rather than by technical artifact.

The main organization follows the ubiquitous language of the domain:

```text
Beridian.Domain
│
├── Common
├── Expenses
├── FinancialPeriods
├── Incomes
├── Investments
├── Events
└── Services
```

Detailed documentation about the concepts contained in these areas belongs to `domain-model/`.

---

## Domain Building Blocks

The Domain layer may contain the following DDD building blocks:

### Aggregate Roots

Define consistency boundaries and protect aggregate invariants.

External layers must use exposed aggregate behavior rather than modifying internal state directly.

### Entities

Represent domain concepts with identity and lifecycle.

### Value Objects

Represent domain concepts defined by their values and are responsible for protecting their own validity.

### Domain Services

Contain business behavior that belongs to the Domain but does not naturally belong to a single Entity or Aggregate Root.

### Domain Events

Represent relevant facts that have occurred within the Domain.

The specific Aggregate Roots, Entities, Value Objects, Domain Services, and Domain Events used by Beridian are documented in `domain-model/`.

---

## Encapsulation

Domain objects must expose behavior instead of unrestricted state mutation.

State changes must occur through domain operations that preserve business rules and invariants.

Outer layers must not directly manipulate the internal state of Aggregates or Entities.

Collections owned by Aggregates should be externally exposed as read-only when appropriate.

---

## Business Rule Enforcement

Business rules and invariants must be enforced by the Domain layer.

Application may request an operation, but the Domain determines whether that operation is valid.

This establishes a fundamental architectural rule:

> **Application coordinates. Domain decides.**

Business validation must not be duplicated in Application, Infrastructure, or API when the rule belongs to the Domain.

---

## Persistence Independence

The Domain model must remain independent from persistence technology.

The Domain must not contain:

* `DbContext` implementations.
* Repository implementations.
* Database queries.
* Connection management.
* Transaction management.
* Persistence-specific behavior.

Infrastructure must adapt persistence mechanisms to the Domain model, not force the Domain model to represent database structures.

---

## Design Principles

The Domain layer follows these principles:

* Keep business behavior inside the Domain.
* Protect invariants at the appropriate consistency boundary.
* Model behavior explicitly rather than exposing mutable state.
* Use domain concepts instead of primitive values when the concept has business meaning.
* Keep Value Object validation within the Value Object.
* Keep aggregate consistency rules within the Aggregate Root.
* Use Domain Services only when behavior does not naturally belong to an Entity or Aggregate Root.
* Keep the Domain independent from Application, Infrastructure, and API.
* Avoid persistence-driven domain modeling.
* Prefer the ubiquitous language in types and operations.

---

## Domain Model Documentation

This document defines the **architectural rules of the Domain layer**.

The actual Beridian business model is documented separately in:

```text
domain-model/
```

That documentation contains the specific:

* Aggregate Roots.
* Entities.
* Value Objects.
* Domain Services.
* Domain Events.
* Business rules.
* Invariants.
* Modeling decisions.
* Domain diagrams.

This separation prevents architectural guidance from being duplicated with business model documentation.

---

## Related Documentation

* `clean-architecture.md`
* `application-layer.md`
* `domain-model/`
* `../adr/ADR-001-adopt-clean-architecture.md`
* `../development/coding-conventions.md`
* `../development/testing-conventions.md`
