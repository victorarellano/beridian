# ADR-003: Adopt Code-First Persistence with EF Core

- **Status:** Accepted
- **Date:** 2026-08-14

---

## Context

Beridian already has a domain model centered on the `FinancialPeriod` aggregate, including entities, value objects, business rules, invariants, and domain events. The persistence mechanism must store this model without making the Domain layer dependent on database or ORM concerns.

The project requires:

- PostgreSQL as its relational database engine.
- A reproducible and version-controlled database schema.
- Compatibility with .NET 8 and the current Clean Architecture dependency rules.
- Explicit mappings for aggregate entities and value objects.
- A controlled mechanism for evolving the schema across development and deployment environments.
- The ability to review database changes before applying them.

Two persistence approaches were considered: Code First and Database First. Because the domain model and its behavior were designed before the persistence schema, making an externally designed database schema the source of the application model would reverse the intended dependency direction.

---

## Decision

Beridian will use a **Code First** persistence approach with **Entity Framework Core 8** and the **Npgsql Entity Framework Core provider**.

**PostgreSQL 16** will be the initial database engine version. Runtime and container configuration must reference this major version explicitly instead of using an unbounded image tag such as `latest`.

The following architectural rules apply:

- The domain model is the source of the application's business structure and behavior.
- Database mappings are implemented in the Infrastructure layer.
- Domain classes must not depend on Entity Framework Core.
- Persistence metadata must be defined with the EF Core Fluent API and `IEntityTypeConfiguration<T>` implementations.
- Persistence-specific attributes such as `[Table]`, `[Column]`, and `[Key]` must not be introduced into the Domain layer.
- EF Core migrations are the source-controlled history of the relational schema.
- Migration files and the EF Core model snapshot must be committed to Git.
- Every generated migration must be reviewed before it is applied or committed.
- Permanent schema changes must not be introduced manually in shared databases.
- A schema change must be represented by a new migration after a previous migration has been applied to a shared environment.
- Applied migrations must be tracked through EF Core's `__EFMigrationsHistory` table.
- Exact package patch versions remain defined in the project files; this ADR governs the selected major technology versions and approach.

The expected evolution flow is:

```text
Domain model change
    -> Infrastructure mapping change
    -> EF Core migration
    -> Migration review
    -> PostgreSQL schema update
```

---

## Consequences

### Positive

- The persistence schema evolves from the application model already defined by the business domain.
- The Domain layer remains independent of EF Core and PostgreSQL.
- Schema changes are reproducible, reviewable, and traceable in Git.
- Developers and deployment environments can apply the same ordered migration history.
- Fluent API mappings allow persistence details to be configured without contaminating domain classes.
- Explicitly selecting PostgreSQL 16 reduces unexpected behavior caused by automatic major-version upgrades.

### Negative

- Domain changes that affect persistence require corresponding mapping and migration work.
- Generated migrations must be inspected because EF Core cannot determine the business intent of every schema change.
- Complex value objects, private collections, and aggregate encapsulation may require detailed EF Core configuration.
- The application team becomes responsible for coordinating migrations across environments.

### Risks

- An incorrectly reviewed migration could cause data loss or an unintended schema change.
- Manual changes to a database could create schema drift from the migration history.
- Updating EF Core, Npgsql, or PostgreSQL major versions without compatibility verification could introduce breaking behavior.
- Automatically applying migrations during application startup could create operational risk in controlled or production environments; the deployment strategy must be defined before production use.

---

## Alternatives Considered

### Database First

Design the PostgreSQL schema first and generate or adapt application classes from that schema.

**Pros**

- Suitable when an existing database is the authoritative source.
- Allows database specialists to control the schema independently.

**Cons**

- Beridian does not currently have a legacy database that must govern the model.
- It would make persistence structure influence the domain model.
- Generated classes could conflict with aggregate encapsulation and domain behavior.
- It is less consistent with the domain-first design already completed.

---

### Manually Managed SQL Migrations

Maintain schema changes through manually authored SQL scripts without EF Core migrations.

**Pros**

- Provides complete control over database-specific SQL.
- Can be appropriate for highly specialized database operations.

**Cons**

- Requires a separate mechanism for generating, ordering, and tracking schema changes.
- Increases the possibility of divergence between EF Core mappings and the physical schema.
- Adds unnecessary operational complexity for the current MVP.

Manual SQL may still be included in an EF Core migration when a specific schema operation cannot be represented safely through the standard migration API.

---

## Related Documentation

- Domain Model
- Clean Architecture
- Infrastructure Layer
- Project Roadmap
- Business Rules
