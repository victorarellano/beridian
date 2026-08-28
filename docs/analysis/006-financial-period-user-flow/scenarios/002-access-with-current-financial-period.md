# Scenario SC-002 — Access With a Current Financial Period

## Purpose

Describe how Beridian behaves when the user accesses the application and a financial period already exists for the current month.

## Trigger

The user opens Beridian.

## Preconditions

- Financial history exists.
- A financial period exists for the current month.
- The current financial period may be open or closed.
- Other financial periods may also remain open.

## Main Flow

1. The user opens Beridian.
2. The system determines the current month and year.
3. The system searches for the financial period corresponding to the current month.
4. The system finds the current financial period.
5. The system retrieves its financial information and current status.
6. The system displays the current financial period.
7. The system makes available the actions permitted by the period status.

## Alternative Flows

### AF-001 — The Current Financial Period Is Closed

1. The system retrieves the current financial period.
2. The system identifies that the period is closed.
3. The system displays the period in read-only mode.
4. The system prevents operations that modify the closed period.

The behavior for continuing to another financial period will be defined in the financial period generation scenario.

### AF-002 — Other Financial Periods Remain Open

1. The system opens the financial period corresponding to the current month.
2. The system does not automatically close any previous financial period.
3. The user can access other open periods through the financial period history.

### AF-003 — The Current Financial Period Cannot Be Retrieved

1. The system detects an unexpected retrieval failure.
2. The system informs the user that the current financial period could not be loaded.
3. The system does not create a replacement period automatically.

## Postconditions

- The financial period corresponding to the current month is displayed.
- The period status is visible to the user.
- Only operations permitted by the period status are available.
- Other open financial periods remain unchanged.

## Business Rules

- More than one financial period may remain open at the same time.
- The current financial period is determined by its month and year.
- Accessing Beridian does not automatically close previous financial periods.
- A closed financial period cannot be modified.
- The current financial period is the default entry point when it exists.

## Backend Capability Requirements

- Retrieve a financial period by month and year.
- Retrieve the complete financial information required by the period overview.
- Identify whether the retrieved financial period is open or closed.
- Retrieve historical financial periods for secondary navigation.

Backend support for these capabilities must be verified during the API contract review.

## Open Questions

- How will the interface indicate that other financial periods remain open?
- Should an open-period notification be displayed when previous periods have not been closed?
- What information must be included in the initial financial period overview?
- What actions should be visible when the current financial period is closed?

## Related Scenarios

- [SC-001 — First Access Without Financial History](001-first-access-without-financial-history.md)
- [SC-003 — Access With History but No Current Financial Period](003-access-with-history-but-no-active-period.md)
- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)