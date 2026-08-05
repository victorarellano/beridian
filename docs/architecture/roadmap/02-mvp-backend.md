# Phase 2 — MVP Backend

## Purpose

Transform the domain model into a functional backend while preserving the business rules defined during the analysis phase.

The objective of this phase is to implement the core domain using Clean Architecture principles, exposing the business capabilities through an API without compromising domain integrity.

The backend should become the executable representation of the business model.

---

## Product Goals

At the end of this phase, the application should allow users to:

- Create and manage financial periods.
- Manage expenses, incomes, and investments.
- Record planned and actual values.
- Close a financial period.
- Generate the next financial period according to the business rules.
- Persist financial information reliably.

Although no user interface exists yet, the complete business workflow should be executable through the API.

---

## Engineering Goals

Implement the backend following Clean Architecture.

During this phase the project should:

- Implement the Domain Layer.
- Implement the Application Layer.
- Design the Infrastructure Layer.
- Expose REST endpoints.
- Configure dependency injection.
- Implement persistence using PostgreSQL.
- Apply repository abstractions.
- Implement domain validations.
- Implement application use cases.
- Separate business logic from infrastructure concerns.

---

## Learning Goals

Develop practical experience implementing modern backend applications using .NET.

Topics covered include:

- Clean Architecture implementation
- CQRS (if adopted)
- Dependency Injection
- Repository Pattern
- Entity Framework Core
- PostgreSQL
- REST API Design
- DTOs
- Validation
- Exception Handling
- Configuration Management

---

## Deliverables

### Domain

- Aggregate implementations
- Entities
- Value Objects
- Domain Services
- Business Invariants

### Application

- Use Cases
- Commands
- Queries
- DTOs
- Interfaces

### Infrastructure

- Entity Framework Core
- PostgreSQL persistence
- Repository implementations
- Dependency Injection
- Configuration

### API

- REST Endpoints
- Request Validation
- Error Handling
- Swagger / OpenAPI documentation

### Testing

- Unit Tests
- Integration Tests
- Business Rule Validation Tests

---

## Exit Criteria

This phase is complete when:

- Every business rule can be executed through the application.
- Financial periods can be created and closed.
- Carry-forward rules work correctly.
- Data persistence is reliable.
- The API exposes all core business capabilities.
- Critical business rules are covered by automated tests.
- The backend is ready to support any future user interface.

---

## Success Indicators

The project team can demonstrate the complete business workflow through the API.

Typical demonstration scenarios include:

- Create a financial period.
- Register expenses and incomes.
- Calculate balances.
- Close the period.
- Generate the next period.
- Verify that carry-forward rules have been applied correctly.

The backend should be fully functional without requiring a graphical user interface.

