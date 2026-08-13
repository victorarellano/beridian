# ADR-002: Organize FinancialPeriod Aggregate Using Partial Classes
- **Status:** Accepted
- **Date:** 2026-08-10

---

## Context
The `FinancialPeriod` Aggregate Root accumulated several responsibilities that correctly belong to the aggregate, including:

- aggregate composition;
- domain operations;
- financial calculations;
- lifecycle closing behavior;
- domain event management.

Although these responsibilities belong to the same aggregate consistency boundary, keeping all implementation details in a single source file made the class increasingly difficult to navigate and maintain.
Extracting these responsibilities into independent domain services or helper classes would reduce the cohesion of the Aggregate Root and could weaken its responsibility for enforcing aggregate invariants.


---

## Decision
Keep `FinancialPeriod` as a single Aggregate Root and organize its implementation using C# partial classes.

The implementation is separated by behavior:

- `FinancialPeriod.cs` — aggregate state, construction and common guards.
- `FinancialPeriod.Composition.cs` — entity composition.
- `FinancialPeriod.Operations.cs` — aggregate operations.
- `FinancialPeriod.Balances.cs` — financial calculations.
- `FinancialPeriod.Closing.cs` — lifecycle closing behavior.
- `FinancialPeriod.Events.cs` — domain event management, if separated.

All partial files declare the same  `FinancialPeriod` class and together represent a single Aggregate Root and a single consistency boundary.

---

## Consequences
- Preserves the cohesion of the Aggregate Root.
- Improves source-code navigation and maintainability.
- Keeps aggregate invariants within the same domain object.
- Avoids introducing artificial helper or service classes solely to reduce file size.
- Makes individual areas of aggregate behavior easier to locate.

### Negative
- The aggregate remains responsible for multiple domain behaviors, even though its implementation is distributed across files.
- Developers must understand that all partial files represent one class.
- Excessive use of partial files could hide genuine responsibility problems if the aggregate continues growing.

---

## Follow-up
The aggregate should be reviewed again if its responsibilities continue to grow.

Partial classes are being used to organize implementation, not as a substitute for correcting an incorrectly defined aggregate boundary.

---

## Alternatives Considered (Optional)
- Extract helper classes.
- Extract domain services.
- Split the aggregate.

These alternatives were rejected because they would reduce aggregate cohesion without solving a domain modeling problem.

---


## Related Documentation
- `docs/architecture/domain-model/aggregates/financial-period.md`
  - Aggregate responsibilities and invariants.

- `docs/architecture/domain-model/domain-services/financial-period-generator.md`
  - Generation process coordinated by the Domain Service.

- `ADR-001-adopt-clean-architecture.md`
  - Architectural principles that guided the domain organization.