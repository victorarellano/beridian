# Backend Gap Analysis

## Purpose

This document identifies the backend changes required to support the Financial Period User Flows included in the current Beridian MVP.
The analysis is based on the Financial Period User Flow to API Mapping and the current API implementation.
Each backend capability is classified according to its current support:

- **Supported** — The current backend provides the capability required by the MVP.
- **Partially Supported** — Related backend functionality exists, but additional behavior or contract changes are required.
- **Missing** — No backend capability supporting the required behavior has been identified.
- **Deferred** — The capability is intentionally outside the scope of the current MVP.

This document identifies required backend changes but does not define their technical implementation.

---

# UF-001 — Application Entry and Period Synchronization

## Gap Analysis

| BC | Required Capability | Current Support | Gap | Required Change |
|---|---|---|---|---|
| BC-005 | Generate missing periods chronologically | Deferred | Outside the current MVP scope. | None for the current MVP. |
| BC-008 | Prevent duplicate periods for the same month and year | Supported | Duplicate-period prevention is enforced by persistence and synchronization reuses an existing period rather than creating a duplicate. | None for the current MVP. |

---

## UF-001 Findings

The application-entry backend gap has been completed through `SynchronizeCurrentFinancialPeriodCommand` and its API endpoint.
The backend now resolves an existing requested current period without requiring the frontend to know its identifier and creates the initial period when no financial history exists.
When financial history exists but the requested current period is missing, the MVP returns a conflict instead of performing chronological catch-up.
Chronological catch-up generation remains intentionally deferred beyond the current MVP.
Duplicate-period prevention has been verified for the relevant MVP creation and synchronization behavior.

# UF-002 — Monthly Financial Period Management

## Gap Analysis

| BC | Required Capability | Current Support | Gap | Required Change |
|---|---|---|---|---|
| BC-009 | Retrieve a complete financial period statement | Supported | `GetFinancialPeriodResult` exposes the complete financial statement required by the current MVP. | None. |
| BC-010 | Retrieve the immediately preceding financial period | Supported | Chronological navigation resolves the target year/month through `GetFinancialPeriodByPeriodQuery`. | None. |
| BC-011 | Retrieve the immediately following financial period | Supported | Chronological navigation resolves the target year/month through `GetFinancialPeriodByPeriodQuery`. | None. |
| BC-012 | Retrieve a specific period by month and year | Supported | `GET /api/v1/financial-periods/by-period` retrieves a financial period by year and month. | None. |
| BC-013 | Retrieve the current financial period from historical navigation | Supported | Current-period navigation resolves the current year/month through the same retrieval-by-period operation. | None. |
| BC-014 | Retrieve expense details | Supported | `GetFinancialPeriodResult` exposes the expense details required by the UI. | None. |
| BC-015 | Add an income to a financial period | Supported | No gap identified. | None. |
| BC-016 | Enter the actual amount of an income | Supported | No gap identified. | None. |
| BC-017 | Retrieve planned and actual income information | Supported | `GetFinancialPeriodResult` exposes planned and actual income amounts. | None. |
| BC-018 | Add a discretionary expense | Supported | No gap identified. | None. |
| BC-019 | Add a fixed-term expense | Supported | No gap identified. | None. |
| BC-020 | Enter an expense using details | Supported | No gap identified. | None. |
| BC-021 | Enter an actual amount for an expense without details | Supported | No gap identified. | None. |
| BC-022 | Retrieve planned and actual expense information | Supported | `GetFinancialPeriodResult` exposes planned and actual expense amounts. | None. |
| BC-023 | Retrieve planned and actual investment | Supported | `GetFinancialPeriodResult` exposes planned and actual investment amounts. | None. |
| BC-024 | Enter or update actual investment | Supported | `ConfirmInvestmentCommand` provides the operation used to register the actual investment amount. | None identified for the current MVP. |
| BC-025 | Retrieve opening and transferred balances | Supported | `GetFinancialPeriodResult` exposes the opening balance transferred into the financial period. | None. |
| BC-026 | Retrieve planned and actual remaining balances | Supported | `GetFinancialPeriodResult` exposes `PlannedBalance` and `ActualBalance`. | None. |

---

## UF-002 Findings

The financial period read contract has been verified and provides the complete statement required by the current MVP, including expense details, planned and actual values for incomes, expenses and investments, and opening, planned, and actual balances.

Chronological financial-period navigation is now supported through `GET /api/v1/financial-periods/by-period`, backed by `GetFinancialPeriodByPeriodQuery`. Previous, next, selected, and current-period navigation resolve the target year and month and use the same backend operation.

# UF-003 — Financial Period Closing

## Gap Analysis

| BC | Required Capability | Current Support | Gap | Required Change |
|---|---|---|---|---|
| BC-028 | Close an open financial period | Supported | No gap identified. | None. |
| BC-029 | Reject modifications to a closed period | Supported | No functional gap identified. Consistency across modification operations should be preserved during implementation. | None for the current MVP. |
| BC-030 | Retrieve the confirmed closed state | Supported | `GetFinancialPeriodResult` exposes the financial period `Status` required by the UI. | None. |

---

## UF-003 Findings

The current backend provides the operation required to close a financial period through `CloseFinancialPeriodCommand`.
The domain protects closed financial periods from subsequent modification, and the API maps the corresponding failure to a conflict response.
`GetFinancialPeriodResult` exposes the financial period `Status`, confirming that the frontend can retrieve and present the resulting closed state.
No confirmed functional or response-contract gap remains in UF-003.

# UF-004 — Financial Period Continuity

## MVP Scope

The complete Financial Period Continuity design includes automatic catch-up, scheduled monthly generation, and multi-period generation failure reporting.

The following capabilities are intentionally deferred beyond the current MVP:

- BC-005 — Generate missing periods chronologically.
- BC-006 — Generate the current period through a scheduled monthly task.
- BC-034 — Identify the month that failed during multi-period generation.

These capabilities remain documented for traceability but are not considered backend gaps for the current MVP.

The current MVP retains manual next-period generation and duplicate-period prevention.

---

## Gap Analysis

| BC | Required Capability | Current Support | Gap | Required Change |
|---|---|---|---|---|
| BC-005 | Generate missing periods chronologically | Deferred | Outside the current MVP scope. | None for the current MVP. |
| BC-006 | Generate the current period through a scheduled monthly task | Deferred | Outside the current MVP scope. | None for the current MVP. |
| BC-007 | Generate the next period manually | Supported | No gap identified. | None. |
| BC-008 | Prevent duplicate periods for the same month and year | Supported | Duplicate-period prevention is enforced by persistence and synchronization reuses an existing period rather than creating a duplicate. | None for the current MVP. |
| BC-034 | Identify the month that failed during multi-period generation | Deferred | Outside the current MVP scope. | None for the current MVP. |

---

## UF-004 Findings

The current backend provides the operation required by the MVP to manually generate the next financial period through `GenerateNextFinancialPeriodCommand`.
No additional backend capability is currently required for automatic chronological catch-up, scheduled monthly generation, or multi-period failure reporting because these behaviors have been explicitly deferred beyond the current MVP.
Duplicate-period prevention remains part of the MVP. Existing backend behavior suggests that duplicate financial periods are already treated as a conflict, but enforcement across the relevant creation and generation paths will be verified during implementation and testing.
Therefore, no confirmed implementation gap has currently been identified for the manual Financial Period Continuity flow.


# MVP Backend Gap Summary

## Confirmed Gaps

No confirmed backend implementation gaps remain for the current MVP capabilities reviewed so far.

The previously identified financial-period retrieval and navigation gaps (BC-010 through BC-013) are now supported through `GetFinancialPeriodByPeriodQuery` and `GET /api/v1/financial-periods/by-period`.

---

## Verification Pending

There aren't capability that requires explicit behavioral verification.

---

## Deferred Beyond MVP

The following capabilities remain part of the broader Financial Period design but are intentionally outside the current MVP scope.

| BC | Deferred Capability |
|---|---|
| BC-005 | Generate missing periods chronologically. |
| BC-006 | Generate the current period through a scheduled monthly task. |
| BC-034 | Identify the month that failed during multi-period generation. |

These capabilities remain documented for future development and traceability.

---

## Supported MVP Capabilities

The current backend already provides the principal command operations required for:

- Financial period creation.
- Application-entry synchronization and current-period resolution.
- Duplicate-period prevention for the relevant MVP paths.
- Income creation and actual amount entry.
- Discretionary and fixed-term expense creation.
- Direct expense entry.
- Expense detail management and entry from details.
- Investment creation and confirmation.
- Financial period closing.
- Manual next-period generation.
- Domain protection against modifications to closed financial periods.
- Complete financial-period statement retrieval.
- Chronological financial-period navigation by year and month.
- Expense-detail and planned/actual financial data retrieval.
- Retrieval of the closed financial-period state.

---

## Conclusion

The current backend provides the transactional and read operations required by the Financial Period MVP capabilities reviewed so far.
The application-entry gaps and the previously identified financial-period retrieval and navigation gaps have been completed.
The financial period response contract has been verified for the dashboard data covered by BC-009, BC-014, BC-017, BC-022, BC-023, BC-025, BC-026, and BC-030.
Capabilities related to automatic continuity and multi-period generation remain explicitly deferred beyond the current MVP.
