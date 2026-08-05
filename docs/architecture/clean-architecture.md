# Clean Architecture

## Purpose

This document describes the initial architecture of Beridian, including the responsibilities of each layer and the allowed dependencies between projects.

## Project Structure

```text
Beridian.Api
Beridian.Application
Beridian.Domain
Beridian.Infrastructure
Beridian.Tests
```

## Dependency Rule

Dependencies must point toward the center of the architecture.

```text
Api ───────────────► Application ───────────────► Domain
 │                         ▲
 │                         │
 └────────────────► Infrastructure
```

The Domain layer must remain independent from frameworks, databases, user interfaces, and infrastructure technologies.

## Layers
## Domain

Contains the core business concepts and rules.

Initial responsibilities:
- Entities
- Value Objects
- Domain rules
- Domain exceptions
- Domain services, when required

The Domain project must not reference any other project.

## Application

Contains the use cases of the system.

Initial responsibilities:
- Commands and queries
- Use cases
- Application interfaces
- DTOs
- Validation
- Orchestration of domain operations

The Application project may reference Domain.

## Infrastructure

Contains technical implementations required by the application.

Initial responsibilities:
- Entity Framework Core
- PostgreSQL persistence
- Repository implementations
- External services
- File export implementations

The Infrastructure project may reference Application and Domain.

## API

Exposes the application through HTTP endpoints.

Initial responsibilities:
- Controllers or endpoints
- Dependency injection configuration
- Authentication and authorization
- HTTP request and response handling
- Application startup and configuration

The API project may reference Application and Infrastructure.

## Tests

Contains automated tests for the solution.

Initial responsibilities:
- Unit tests
- Integration tests
- Architecture tests

Test references will be added according to the type of test being implemented.

Allowed Project References
Beridian.Application
└── Beridian.Domain

Beridian.Infrastructure
├── Beridian.Application
└── Beridian.Domain

Beridian.Api
├── Beridian.Application
└── Beridian.Infrastructure
Prohibited Dependencies
Domain must not reference Application.
Domain must not reference Infrastructure.
Domain must not reference API.
Application must not reference Infrastructure.
Application must not reference API.
Infrastructure must not reference API.
