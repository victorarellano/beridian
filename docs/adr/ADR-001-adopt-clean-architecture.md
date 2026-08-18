# ADR-001: Adopt Clean Architecture
- Status: Accepted
- Date: 2026-07-10

## Context

Beridian is intended to evolve from an initial financial management MVP into a production-oriented platform that incorporates PostgreSQL, observability, Kubernetes, artificial intelligence, and external integrations.

The application requires a structure that keeps business rules independent from frameworks, persistence technologies, delivery mechanisms, and infrastructure details.

## Decision

Beridian will use Clean Architecture with the following projects:

- `Beridian.Domain`
- `Beridian.Application`
- `Beridian.Infrastructure`
- `Beridian.Api`
- `Beridian.Domain.Tests`

Dependencies must point toward the core of the application.

Allowed production dependencies:

```text
Application    → Domain
Infrastructure → Application, Domain
Api → Application
Api → Infrastructure only as the Composition Root

The API may reference Infrastructure for dependency registration, but endpoints must not directly depend on infrastructure implementations.
```

Domain must not reference any other project.

Test dependencies will be configured according to the type of test being implemented.

## Consequences
### Positive
- Business rules remain independent from infrastructure technologies.
- PostgreSQL and Entity Framework Core can be changed with limited impact on the application logic.
- Use cases can be tested independently.
- Architectural responsibilities are explicitly separated.
- The solution is prepared for controlled technical evolution.

### Negative
- The solution contains more projects and abstractions than a simple CRUD application.
- Developers must understand and respect the dependency rule.
- Mapping between API, domain, and persistence models may become necessary as the system evolves.
- Related Documentation
- Clean Architecture

## Related Documentation
- [Clean Architecture](https://chatgpt.com/g/g-p-6a45b76915a08191a147396af322a00c/c/6a4f89c9-0760-83e9-aecb-ad314559a101#:~:text=Related%20Documentation-,Clean%20Architecture,-%23%23%202.%20Verificar%20la)


