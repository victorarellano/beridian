# Financial Period User Flow

## Purpose

This section documents the user flows involved in managing a financial period in Beridian.

The analysis uses a scenario-based approach to describe how the user interacts with the application under different financial period states. These scenarios will guide the subsequent screen design, API contract review, frontend implementation, and presentation-layer testing.

## Scope

This analysis covers the user journey from the initial access to Beridian through the completion of a financial period and the generation of the next one.

It includes:

- Initial application access.
- Active financial period access.
- Financial history access.
- Financial period overview.
- Income management.
- Expense management.
- Investment and balance management.
- Financial period closing.
- Next financial period generation.

Detailed screen layouts, Angular components, API contracts, and implementation decisions are outside the scope of this analysis.

## Analysis Approach

The user flow is documented using **Scenario-Based User Flow Design**.

Each scenario describes:

- Purpose.
- Trigger.
- Preconditions.
- Main flow.
- Alternative flows.
- Postconditions.
- Related business rules.
- Backend capability gaps.
- Open questions.

The resulting scenarios will provide traceability across the presentation implementation:

```text
Scenario → User Flow → Screen → API Operation → Use Case → Test
```

## Scenario Catalogue

|ID	Scenario|Description|Status|
|-----------|-----------|------|
|SC-001|[First Access Without Financial History	Automatically initializes Beridian when no financial history exists.](scenarios/001-first-access-without-financial-history.md)|Done|
|SC-002|[Access With an Current Financial Period	Opens the financial period currently available for user operations.](scenarios/002-access-with-current-financial-period.md)|Done|
|SC-003|[Access With History but No Current Period	Determines the entry behavior when previous periods exist but none is active.](scenarios/003-access-with-history-but-no-current-period.md)|Done|
|SC-004|[Financial Period Overview	Presents the information and actions required to manage the active period.](scenarios/004-financial-period-overview.md)|Done|
|SC-005|[Income Management Flow	Describes how planned and actual incomes are reviewed and registered.](scenarios/005-income-management-flow.md)|Done|
|SC-006|[Expense Management Flow	Describes how supported expenses are reviewed, created, and entered.](scenarios/006-expense-management-flow.md)|Done|
|SC-007|[Investment and Balance Flow	Describes how investments and available balances are presented and managed.](scenarios/007-investment-and-balance-flow.md)|Done|
|SC-008|[Financial Period Closing Flow	Describes how an active financial period is reviewed and closed.](scenarios/008-financial-period-closing-flow.md)|Done|
|SC-009|[Next Financial Period Generation Flow	Describes how the next financial period is generated from the closed period.](scenarios/009-next-financial-period-generation-flow.md)|Done|

## Confirmed Decisions

The following decisions have already been established:

1. When no financial history exists, Beridian automatically creates an empty financial period for the current month.
2. The automatically created financial period is open.
3. The current financial period is presented to the user.
4. When an active financial period exists, Beridian opens it automatically.
5. Financial period history is available as a secondary navigation option.
6. Existing spreadsheet data migration is required but deferred to a later Sprint 3 session.
7. A scheduled background task runs on the first day of each month and generates the new current financial period from the immediately preceding period.
8. If scheduled generation does not complete, Beridian restores continuity through automatic catch-up or manual generation.
9. Scheduled, automatic, and manual generation must not create duplicate financial periods.
10. Historical financial periods are accessed from the main dashboard through a financial period navigation bar.
11. The navigation bar allows the user to move to the previous period, move to the next period, select a specific month and year, and return to the current period.
12. Navigating between financial periods does not modify their financial information.

## Deferred Requirement
### Existing Spreadsheet Data Migration

Beridian must eventually import the financial history currently maintained in the owner's spreadsheet.
The migration analysis must determine how imported data interacts with an automatically created current financial period, including whether the period should be replaced, merged, or preserved.
This requirement is recorded here because it affects initial application state, but its detailed design and implementation are outside the scope of this user-flow session.

## End-to-End User Flow

The end-to-end financial period experience is documented through four
complementary user flows.

| ID | User Flow | Purpose |
|---|---|---|
| UF-001 | [Application Entry and Period Synchronization](flows/001-application-entry-and-period-synchronization.md) | Ensures that the user reaches the financial period corresponding to the current month. |
| UF-002 | [Monthly Financial Period Management](flows/002-monthly-financial-period-management.md) | Allows the user to review and manage incomes, expenses, investment, and balances. |
| UF-003 | [Financial Period Closing](flows/003-financial-period-closing.md) | Allows the user to review and manually close an open financial period. |
| UF-004 | [Financial Period Continuity](flows/004-financial-period-continuity.md) | Maintains monthly continuity by generating the next required financial periods. |

Financial period continuity can be initiated through:

- Scheduled monthly generation.
- Automatic catch-up during application entry.
- Manual generation initiated by the user.

Together, these flows describe the complete financial period journey:

1. Beridian identifies or creates the financial period required for the current month through UF-001.
2. The user reviews and manages the financial statement through UF-002.
3. The user manually closes the financial period through UF-003.
4. Beridian generates the next required period or restores missing monthly continuity through UF-004.
5. The user returns to the financial period overview and continues the monthly management cycle.

Each user flow distinguishes:

- Operation paths.
- Valid alternative paths.
- Domain validation failures.
- Technical failures.
- Input validation failures when user-provided data is involved.
- The final confirmed state.

The flows maintain the following traceability:

```text
Scenario → User Flow → Screen → API Operation → Use Case → Test
```

## Backend Capability Inventory

This inventory identifies the backend capabilities required by the documented scenarios and user flows.

It does not define the final API contract. Endpoint availability, request and response models, and error mappings will be verified during the frontend architecture and API contract review session.

### Status Definitions

| Status | Meaning |
|---|---|
| Confirmed | The capability has already been identified in the current implementation. |
| Domain Confirmed | The domain supports the behavior, but application and API support still require verification. |
| To Verify | Support cannot yet be confirmed from the current capability inventory. |
| Required | The user flows require the capability, but no implementation has been confirmed. |

### Application Entry and Period Continuity

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-001 | Determine whether financial history exists | UF-001 | To Verify | Review repository and API queries |
| BC-002 | Retrieve the financial period for the current month | UF-001 | To Verify | Define or verify current-period query |
| BC-003 | Create the initial current financial period when no history exists | SC-001 / UF-001 | Required | Define application use case and API behavior |
| BC-004 | Retrieve the latest available financial period | SC-003 / UF-001 | To Verify | Define or verify latest-period query |
| BC-005 | Generate missing periods chronologically | SC-003 / UF-001 / UF-004 | Domain Confirmed | Verify application orchestration and persistence |
| BC-006 | Generate the current period through a scheduled monthly task | UF-004 | Required | Design background-task execution |
| BC-007 | Generate the next period manually | SC-009 / UF-004 | Domain Confirmed | Verify or implement application use case |
| BC-008 | Prevent duplicate periods for the same month and year | UF-001 / UF-004 | To Verify | Verify domain and database enforcement |

### Financial Period Retrieval and Navigation

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-009 | Retrieve a complete financial period statement | SC-004 / UF-002 | To Verify | Define the required query response |
| BC-010 | Retrieve the immediately preceding financial period | SC-004 / UF-002 | Required | Define navigation query |
| BC-011 | Retrieve the immediately following financial period | SC-004 / UF-002 | Required | Define navigation query |
| BC-012 | Retrieve a specific period by month and year | SC-004 / UF-002 | To Verify | Verify or define period search |
| BC-013 | Retrieve the current financial period from historical navigation | SC-004 / UF-002 | To Verify | Reuse or define current-period query |
| BC-014 | Retrieve expense details | SC-004 / SC-006 / UF-002 | To Verify | Verify detail retrieval support |

### Income Management

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-015 | Add an income to a financial period | SC-005 / UF-002 | Confirmed | Verify API contract |
| BC-016 | Enter the actual amount of an income | SC-005 / UF-002 | Confirmed | Verify API contract |
| BC-017 | Retrieve planned and actual income information | SC-004 / SC-005 / UF-002 | To Verify | Include in financial statement query |

### Expense Management

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-018 | Add a discretionary expense | SC-006 / UF-002 | Confirmed | Verify API contract |
| BC-019 | Add a fixed-term expense | SC-006 / UF-002 | Confirmed | Verify API contract |
| BC-020 | Enter an expense using details | SC-006 / UF-002 | Confirmed | Verify API contract |
| BC-021 | Enter an actual amount for an expense without details | SC-006 / UF-002 | Required | Verify domain support and define use case |
| BC-022 | Retrieve planned and actual expense information | SC-004 / SC-006 / UF-002 | To Verify | Include in financial statement query |

### Investment and Balances

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-023 | Retrieve planned and actual investment | SC-007 / UF-002 | To Verify | Include in financial statement query |
| BC-024 | Enter or update actual investment | SC-007 / UF-002 | Required | Define application use case |
| BC-025 | Retrieve opening and transferred balances | SC-004 / SC-007 / UF-002 | To Verify | Include in financial statement query |
| BC-026 | Retrieve planned and actual remaining balances | SC-004 / SC-007 / UF-002 | To Verify | Confirm calculation ownership |
| BC-027 | Reflect unexpected expenses in actual investment | SC-006 / SC-007 / UF-002 | Domain Confirmed | Verify response after expense operation |

### Financial Period Closing

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-028 | Close an open financial period | SC-008 / UF-003 | Domain Confirmed | Verify or implement application use case |
| BC-029 | Reject modifications to a closed period | SC-005–SC-008 / UF-002 / UF-003 | Confirmed | Verify consistent API error mapping |
| BC-030 | Retrieve the confirmed closed state | SC-008 / UF-003 | To Verify | Include status in financial statement query |

### Error Handling

| ID | Required Capability | Related Flow | Current Status | Next Action |
|---|---|---|---|---|
| BC-031 | Return input-validation errors | UF-002 | To Verify | Review request validation strategy |
| BC-032 | Return domain-validation failures separately from technical failures | UF-001–UF-004 | To Verify | Review exception mapping |
| BC-033 | Return a consistent technical failure response | UF-001–UF-004 | To Verify | Review API error contract |
| BC-034 | Identify the month that failed during multi-period generation | SC-003 / SC-009 / UF-004 | Required | Define generation error result |

## Known Implementation Evidence

The current MVP inventory confirms application handlers for:

- Adding an income.
- Entering an actual income amount.
- Adding a discretionary expense.
- Adding a fixed-term expense.
- Entering an expense using details.

The domain model confirms behavior for:

- Closing a financial period.
- Generating the next financial period.
- Applying carry-forward and reset rules.
- Preventing modifications to closed financial periods.
- Adjusting actual investment after an unexpected expense.

Application handlers, API endpoints, query models, persistence enforcement, and
error contracts not explicitly confirmed above must be verified before frontend
integration.

## Open Questions

Open questions identified during scenario analysis will be consolidated in this section.
Initial questions include:

- What should Beridian do when financial history exists but no current financial period is available?
  R. Beridian must always provide a financial period for the current month. A scheduled background task runs on the first day of each month and generates the new financial period from the immediately preceding period.
- When should the next financial period be generated?
  R. The next financial period is generated automatically by a scheduled background task on the first day of each month
- How should spreadsheet migration handle an automatically created current period?
  R. Spreadsheet migration is outside the scope of Sprint 3.

## Related Documentation
Current Business Process
Domain Discovery
Business Rules
Domain Assumptions
Open Questions