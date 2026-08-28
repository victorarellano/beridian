# Scenario SC-003 — Access With History but No Current Financial Period

## Purpose

Describe how Beridian restores financial period continuity when financial history exists but no financial period has been created for the current month.

## Trigger

The user opens Beridian.

## Preconditions

- At least one historical financial period exists.
- No financial period exists for the current month.
- The latest available financial period belongs to a month before the current month.

## Scheduled Generation Context

Under normal operation, a scheduled background task runs on the first day of each month and generates the new current financial period from the immediately preceding period.

This scenario acts as a recovery mechanism when:

- The scheduled task did not run.
- The scheduled task failed.
- The application was unavailable when generation was expected.
- More than one financial period is missing.

Beridian must still restore continuity until the current financial period exists.

## Main Flow

1. The user opens Beridian.
2. The system determines the current month and year.
3. The system verifies that no financial period exists for the current month.
4. The system retrieves the latest available financial period.
5. The system identifies every missing month between the latest available period and the current month.
6. The system generates each missing financial period in chronological order.
7. Each new period is generated from its immediately preceding financial period according to the defined period-generation rules.
8. The system continues generating periods until a financial period exists for the current month.
9. The system displays the current financial period.
10. The user can begin working with the current financial period.

## Alternative Flows

### AF-001 — Only the Current Month Is Missing

1. The system retrieves the financial period for the immediately preceding month.
2. The system generates the current financial period from it.
3. The system displays the generated current period.

### AF-002 — Multiple Consecutive Months Are Missing

1. The system identifies all missing months.
2. The system generates the oldest missing period first.
3. The system uses each generated period as the source for the following period.
4. The system repeats the process until it generates the current financial period.
5. The system displays the current financial period.

### AF-003 — A Missing Period Cannot Be Generated

1. The system stops the generation sequence at the first failed period.
2. The system does not skip the failed month.
3. The system informs the user that the financial history could not be brought up to date.
4. The system identifies the period that could not be generated.

Whether the complete operation is rolled back or successfully generated periods are preserved remains an implementation decision.

### AF-004 — The Latest Available Period Is Open

1. The system does not close the latest available period automatically.
2. The system generates the missing periods according to the financial period generation rules.
3. Previously open financial periods remain open.

## Postconditions

- A continuous sequence of financial periods exists up to the current month.
- A financial period exists for every previously missing month.
- The financial period for the current month is displayed.
- Previously open financial periods remain unchanged.
- The user is not required to create each missing period manually.

## Business Rules

- Financial periods are generated in chronological order.
- A missing month cannot be skipped during automatic period generation.
- Each generated period uses its immediately preceding period as its source.
- More than one financial period may remain open at the same time.
- Generating missing periods does not automatically close previous periods.
- The generation of each period must comply with the existing domain rules.
- The current financial period is the default entry point after synchronization.

## Backend Capability Requirements

- Determine whether a financial period exists for a specific month and year.
- Retrieve the latest available financial period.
- Generate the next financial period from an existing period.
- Generate several consecutive financial periods safely.
- Return the period in which a generation failure occurred.
- Retrieve the generated current financial period.

Backend support for these capabilities must be verified during the API contract review.

## Open Questions

- Should the interface show progress when several periods must be generated?
- Should the system notify the user which periods were generated automatically?
- Should automatically generated intermediate periods be visually identified?
- How should partial generation be resumed after a failure?
- Should multi-period generation be atomic, or should successfully generated periods be preserved after a failure?
- How should period generation behave when the existing financial history came from the spreadsheet migration?

## Related Scenarios

- [SC-001 — First Access Without Financial History](001-first-access-without-financial-history.md)
- [SC-002 — Access With a Current Financial Period](002-access-with-current-financial-period.md)
- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)
