# Financial Period User Flow to API Mapping

## Purpose

This document maps the backend capabilities required by the Financial Period User Flows to the API operations currently available in Beridian.
The mapping establishes traceability between the designed user experience, backend capabilities, API operations, and application use cases.
This document does not determine how missing or partially supported capabilities will be implemented. Backend gaps and required changes are analyzed separately during the backend gap analysis.

---

## Mapping Structure

The mapping follows the traceability established by the Financial Period User Flow analysis:

```text
Scenario → User Flow → Screen → API Operation → Use Case → Test
```

For this document, the relevant portion is represented as:

```text
User Flow → Backend Capability → API Operation → Application Use Case
```

When no API operation corresponding to a required capability has been identified, the capability is marked as **Not Mapped**.
A mapped API operation indicates that an existing operation is related to the required capability. It does not by itself confirm that the complete behavior required by the User Flow is supported.

---

# UF-001 — Application Entry and Period Synchronization

## Purpose

Ensure that the user reaches the financial period corresponding to the current month when entering Beridian.
The flow must handle different application states, including:

- No existing financial history.
- An existing current financial period.
- Existing financial history without a current financial period.
- Missing periods between the latest available period and the current month.

---

## API Mapping

| Backend Capability | Required Capability | API Operation | Application Use Case | Mapping |
|---|---|---|---|---|
| BC-001 | Determine whether financial history exists | `POST /api/v1/financial-periods/synchronize` | `SynchronizeCurrentFinancialPeriodCommand` | Mapped |
| BC-002 | Retrieve the financial period for the current month | `POST /api/v1/financial-periods/synchronize` | `SynchronizeCurrentFinancialPeriodCommand` | Mapped |
| BC-003 | Create the initial current financial period when no history exists | `POST /api/v1/financial-periods/synchronize` | `SynchronizeCurrentFinancialPeriodCommand` | Mapped |
| BC-004 | Retrieve the latest available financial period | `POST /api/v1/financial-periods/synchronize` | `SynchronizeCurrentFinancialPeriodCommand` | Mapped |
| BC-005 | Generate missing periods chronologically | — | — | Not Mapped |
| BC-008 | Prevent duplicate periods for the same month and year | Synchronization and persistence enforcement | Backend enforcement | Mapped |

---

## Mapping Notes

### BC-001 — Determine Whether Financial History Exists

The API now provides `POST /api/v1/financial-periods/synchronize` through `SynchronizeCurrentFinancialPeriodCommand`.
The synchronization use case determines whether financial history exists as part of application-entry resolution.

**Mapping:** Mapped.

---

### BC-002 — Retrieve the Financial Period for the Current Month

The synchronization endpoint resolves the requested current period by year and month without requiring the frontend to already know its identifier.

**Mapping:** Mapped.

---

### BC-003 — Create the Initial Current Financial Period When No History Exists

The synchronization endpoint creates the requested current financial period when no financial history exists.
The result indicates whether the period was created through `WasCreated`.

**Mapping:** Mapped.

---

### BC-004 — Retrieve the Latest Available Financial Period

Repository support for retrieving the latest available financial period has been implemented and verified as part of the application-entry backend capability set.
The public application-entry operation is `POST /api/v1/financial-periods/synchronize`.

**Mapping:** Mapped.

---

### BC-005 — Generate Missing Periods Chronologically

The existing API provides:
`POST /api/v1/financial-periods/{financialPeriodId}/next`
through:
`GenerateNextFinancialPeriodCommand`
This operation generates the next financial period from a specific existing financial period.
However, BC-005 requires generation of missing periods chronologically when more than one period may be absent.
No API operation representing that complete orchestration has been identified.
The existing `GenerateNextFinancialPeriod` operation may participate in such an orchestration, but it is not mapped as equivalent to BC-005.

**Mapping:** Not Mapped.

---

### BC-008 — Prevent Duplicate Periods for the Same Month and Year

This capability remains a backend invariant rather than a separate user-initiated operation.
Persistence prevents duplicate financial periods for the same month and year, and synchronization returns the existing period when the requested period already exists instead of creating another one.
The relevant MVP behavior has been verified through repository and HTTP integration tests.

**Mapping:** Mapped.

---

## UF-001 Mapping Summary

| Mapping | Count |
|---|---:|
| Mapped | 5 |
| Not Mapped | 1 |
| **Total** | **6** |

UF-001 now maps the MVP application-entry capabilities to the synchronization API and its application use case.
BC-005 remains the only unmapped UF-001 capability and is intentionally deferred beyond the current MVP.

---

## Pending User Flows

There are no user flows pending mapping.

---

## Related Documentation

- Financial Period User Flow
- API Endpoint Inventory
- Financial Period Dashboard
- Screen Map
- Interface States


# UF-002 — Monthly Financial Period Management

## Purpose

Allow the user to review and manage the financial information of a financial period, including navigation, incomes, expenses, investments, and balances.

The flow covers both the retrieval of the financial statement and the operations performed by the user while managing an open financial period.

---

## API Mapping

| Backend Capability | Required Capability | API Operation | Application Use Case | Mapping |
|---|---|---|---|---|
| BC-009 | Retrieve a complete financial period statement | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-010 | Retrieve the immediately preceding financial period | `GET /api/v1/financial-periods/by-period` | `GetFinancialPeriodByPeriodQuery` | Mapped |
| BC-011 | Retrieve the immediately following financial period | `GET /api/v1/financial-periods/by-period` | `GetFinancialPeriodByPeriodQuery` | Mapped |
| BC-012 | Retrieve a specific period by month and year | `GET /api/v1/financial-periods/by-period` | `GetFinancialPeriodByPeriodQuery` | Mapped |
| BC-013 | Retrieve the current financial period from historical navigation | `GET /api/v1/financial-periods/by-period` | `GetFinancialPeriodByPeriodQuery` | Mapped |
| BC-014 | Retrieve expense details | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-015 | Add an income to a financial period | `POST /api/v1/financial-periods/{financialPeriodId}/incomes` | `AddIncomeCommand` | Mapped |
| BC-016 | Enter the actual amount of an income | `POST /api/v1/financial-periods/{financialPeriodId}/incomes/{incomeId}/entry` | `EnterIncomeCommand` | Mapped |
| BC-017 | Retrieve planned and actual income information | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-018 | Add a discretionary expense | `POST /api/v1/financial-periods/{financialPeriodId}/expenses/discretionary` | `AddDiscretionaryExpenseCommand` | Mapped |
| BC-019 | Add a fixed-term expense | `POST /api/v1/financial-periods/{financialPeriodId}/expenses/fixed-term` | `AddFixedTermExpenseCommand` | Mapped |
| BC-020 | Enter an expense using details | `POST /api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry-from-details` | `EnterExpenseUsingDetailsCommand` | Mapped |
| BC-021 | Enter an actual amount for an expense without details | `POST /api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry` | `EnterExpenseCommand` | Mapped |
| BC-022 | Retrieve planned and actual expense information | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-023 | Retrieve planned and actual investment | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-024 | Enter or update actual investment | `POST /api/v1/financial-periods/{financialPeriodId}/investments/{investmentId}/confirmation` | `ConfirmInvestmentCommand` | Mapped |
| BC-025 | Retrieve opening and transferred balances | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |
| BC-026 | Retrieve planned and actual remaining balances | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |

---

## Mapping Notes

### Financial Period Retrieval and Navigation

`GetFinancialPeriodQuery` provides the financial statement when the financial period identifier is already known.

Chronological navigation is supported through `GET /api/v1/financial-periods/by-period?year={year}&month={month}` and `GetFinancialPeriodByPeriodQuery`.

The frontend resolves the chronological target year and month for previous, next, selected, and current-period navigation. The backend retrieves that exact period and does not skip missing months.

Therefore BC-009 through BC-014 are mapped.

Expense details and the financial statement data required by the current MVP have been verified in `GetFinancialPeriodResult`.

---

### Income Management

The current API provides operations for:

- Adding an income.
- Entering its actual amount.
- Retrieving income information as part of the financial period statement.

Therefore BC-015, BC-016, and BC-017 are mapped to existing API and application operations.

---

### Expense Management

The current API provides operations for:

- Adding discretionary expenses.
- Adding fixed-term expenses.
- Entering an actual expense amount directly.
- Adding expense details.
- Entering an expense from its details.
- Retrieving expense information through the financial period statement.

Therefore BC-018 through BC-022 have corresponding operations in the current API.

The API also exposes `AddRecurringExpense`, although recurring-expense creation is not represented as an independent capability in BC-018 through BC-022.

---

### Investment and Balances

The financial period query provides investment and balance information as part of the financial statement.
The API also provides investment creation and confirmation operations.
BC-024 maps to the existing investment confirmation operation because that operation records the actual investment amount.

---

## UF-002 Mapping Summary

| Mapping | Count |
|---|---:|
| Mapped | 18 |
| Not Mapped | 0 |
| **Total** | **18** |

UF-002 now maps all required capabilities to existing API operations, application use cases, or domain behavior.

Financial-period navigation uses chronological year/month resolution through `GET /api/v1/financial-periods/by-period`, while income, expense, investment, and balance information is available through the financial-period statement.


# UF-003 — Financial Period Closing

## Purpose

Allow the user to review and manually close an open financial period once the conditions required for closing have been satisfied.

The flow covers the closing operation, protection of closed financial periods, and retrieval of the resulting closed state.

---

## API Mapping

| Backend Capability | Required Capability | API Operation | Application Use Case | Mapping |
|---|---|---|---|---|
| BC-028 | Close an open financial period | `POST /api/v1/financial-periods/{financialPeriodId}/close` | `CloseFinancialPeriodCommand` | Mapped |
| BC-029 | Reject modifications to a closed period | Existing modification endpoints | Domain enforcement | Mapped |
| BC-030 | Retrieve the confirmed closed state | `GET /api/v1/financial-periods/{financialPeriodId}` | `GetFinancialPeriodQuery` | Mapped |

---

## Mapping Notes

### BC-028 — Close an Open Financial Period

The current API provides:

`POST /api/v1/financial-periods/{financialPeriodId}/close`

through:

`CloseFinancialPeriodCommand`

This operation corresponds directly to the capability required to close an open financial period.

**Mapping:** Mapped.

---

### BC-029 — Reject Modifications to a Closed Period

This capability represents domain enforcement rather than an independent API operation.

The reviewed backend identifies closed financial periods as a conflict condition and maps the corresponding domain failure to an HTTP conflict response.

Modification operations therefore rely on domain enforcement to prevent changes to a closed financial period.

**Mapping:** Mapped.

The consistency of this behavior across all modification operations will be evaluated during the backend gap analysis.

---

### BC-030 — Retrieve the Confirmed Closed State

The existing API provides:

`GET /api/v1/financial-periods/{financialPeriodId}`

through:

`GetFinancialPeriodQuery`

This query retrieves the financial period after the closing operation.

`GetFinancialPeriodResult` exposes the financial period status required by the UI after closing.
The response contract has been verified for this capability.

**Mapping:** Mapped.

---

## UF-003 Mapping Summary

| Mapping | Count |
|---|---:|
| Mapped | 3 |
| Not Mapped | 0 |
| **Total** | **3** |

UF-003 has corresponding backend operations or domain behavior for all three required capabilities.

The financial period response contract has been verified to expose the closed state required by the UI.




# UF-004 — Financial Period Continuity

## Purpose

Maintain financial period continuity by generating the next required financial periods from the immediately preceding period.

Continuity can be initiated through:

- Scheduled monthly generation.
- Automatic catch-up during application entry.
- Manual generation initiated by the user.

Generation must preserve chronological continuity and must not create duplicate financial periods.

---

## MVP Scope Decision

The following capabilities are part of the complete Financial Period Continuity design but are intentionally deferred beyond the current MVP:

- BC-005 — Generate missing periods chronologically.
- BC-006 — Generate the current period through a scheduled monthly task.
- BC-034 — Identify the month that failed during multi-period generation.

These capabilities remain documented to preserve traceability but are not considered backend gaps for the current MVP.

Manual next-period generation through BC-007 remains part of the MVP.

Duplicate-period prevention (BC-008) remains subject to verification during implementation.


## API Mapping

| Backend Capability | Required Capability | API Operation | Application Use Case | Mapping | MVP Scope |
|---|---|---|---|---|---|
| BC-005 | Generate missing periods chronologically | — | — | Not Mapped | Deferred |
| BC-006 | Generate the current period through a scheduled monthly task | — | — | Not Mapped | Deferred |
| BC-007 | Generate the next period manually | `POST /api/v1/financial-periods/{financialPeriodId}/next` | `GenerateNextFinancialPeriodCommand` | Mapped | In Scope |
| BC-008 | Prevent duplicate periods for the same month and year | Persistence and synchronization enforcement | Backend enforcement | Mapped | In Scope |
| BC-034 | Identify the month that failed during multi-period generation | — | — | Not Mapped | Deferred |

---

## Mapping Notes

### BC-005 — Generate Missing Periods Chronologically

The current API provides an operation for generating the next financial period from a specific existing financial period.

However, no API or application operation has been identified that orchestrates the generation of multiple missing financial periods chronologically.

The existing `GenerateNextFinancialPeriodCommand` may be used as part of such an orchestration, but it does not map directly to the complete capability required by BC-005.

**Mapping:** Not Mapped.

---

### BC-006 — Generate the Current Period Through a Scheduled Monthly Task

No scheduled background execution mechanism has been identified in the reviewed API.

The existing next-period generation operation can generate a financial period, but it does not represent the scheduled execution required by this capability.

**Mapping:** Not Mapped.

---

### BC-007 — Generate the Next Period Manually

The current API provides:

`POST /api/v1/financial-periods/{financialPeriodId}/next`

through:

`GenerateNextFinancialPeriodCommand`

This operation allows generation of the next financial period from a specified existing period and therefore corresponds to the manual generation capability.

**Mapping:** Mapped.

---

### BC-008 — Prevent Duplicate Periods for the Same Month and Year

Duplicate-period prevention has been verified for the relevant MVP paths.

Persistence rejects duplicate financial periods, and the synchronization operation returns the existing period when the requested month and year already exist.

**Mapping:** Mapped.

---

### BC-034 — Identify the Month That Failed During Multi-Period Generation

No multi-period generation operation has been identified in the current API.

Consequently, no result or error contract has been identified that reports which month failed during chronological generation.

**Mapping:** Not Mapped.

---

## UF-004 Mapping Summary

| Mapping | Count |
|---|---:|
| Mapped | 2 |
| Not Mapped | 3 |
| **Total** | **5** |

The current backend provides the operation required for explicit generation of the next financial period.

Scheduled generation, chronological multi-period catch-up, and failure identification do not currently map to API operations identified in the existing endpoint inventory.

Duplicate-period prevention has been verified for the relevant MVP creation and synchronization paths.