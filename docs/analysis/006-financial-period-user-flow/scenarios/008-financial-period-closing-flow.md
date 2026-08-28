# Scenario SC-008 — Financial Period Closing Flow

## Purpose

Describe how the user manually closes an open financial period and how Beridian presents the resulting closed state.

## Trigger

The user starts the close-period operation from an open financial period.

## Preconditions

- The selected financial period exists.
- The selected financial period is open.
- The financial period information required for review is available.

## Main Flow

1. The user reviews the planned and actual financial information of the open period.
2. The user starts the close-period operation.
3. The system informs the user that closing the period will prevent further modifications.
4. The user confirms the operation.
5. The system requests that the financial period be closed.
6. The domain validates the closing operation.
7. The system marks the financial period as closed.
8. The system refreshes the financial period overview.
9. The period remains visible in consultation mode.
10. All operations that modify the closed period become unavailable.

## Closing Review

Before confirmation, the user must be able to review the financial statement of the period, including:

- Planned and actual balances.
- Planned and actual incomes.
- Planned and actual expenses.
- Planned and actual investment.
- Expense details when available.

The review provides information for the user's decision but does not modify the period automatically.

## Alternative Flows

### AF-001 — The User Cancels the Closing Operation

1. The user starts the close-period operation.
2. The system presents the closing confirmation.
3. The user cancels the operation.
4. The financial period remains open and unchanged.
5. The user returns to the financial period overview.

### AF-002 — Domain Validation Prevents Closing

1. The user confirms the close-period operation.
2. The domain rejects the operation because an applicable business rule is not satisfied.
3. The system keeps the financial period open.
4. The system communicates the reason the period could not be closed.
5. The user returns to the period without losing existing information.

The specific closing preconditions must be verified against the current domain implementation.

### AF-003 — The Period Was Already Closed

1. The system detects that the financial period is already closed.
2. The system does not execute the closing operation again.
3. The system displays the period in consultation mode.

### AF-004 — The Backend Operation Fails

1. The system informs the user that the financial period could not be closed.
2. The interface does not present the period as closed without confirmation from the backend.
3. The system preserves the last confirmed period state.
4. The user may retry the operation.

### AF-005 — Other Financial Periods Remain Open

1. The system closes only the selected financial period.
2. Other open financial periods remain unchanged.
3. The existence of other open periods does not cause them to close automatically.

## Postconditions

- The selected financial period is closed after a successful operation.
- The period remains available for consultation.
- The closed period cannot receive further modifying operations.
- Other financial periods remain unchanged.
- Closing the period does not automatically imply that another period was closed.

## Business Rules

- Financial period closing is initiated manually by the user.
- Only an open financial period can be closed.
- A closed financial period cannot be modified.
- Closing one financial period does not close other open financial periods.
- More than one financial period may remain open at the same time.
- The closing operation must comply with the current domain rules.

## Backend Capability Requirements

The API contract review must verify:

- Retrieval of the complete financial period information required for closing review.
- An application operation for closing a financial period.
- Domain validation errors returned by the closing operation.
- Retrieval of the updated closed state.
- Rejection of modifying operations against a closed period.

No implemented close-period command or handler has yet been confirmed in the current MVP inventory.

## Open Questions

- Are there mandatory conditions that must be satisfied before a period can be closed?
- Can a period be closed while incomes or expenses remain unentered?
- Should Beridian warn about incomplete financial information without preventing closure?
- Should the closing confirmation display a final balance summary?
- Can a closed period ever be reopened, or is closing irreversible?
- When should Beridian offer generation of the following financial period?

## Related Scenarios

- [SC-002 — Access With a Current Financial Period](002-access-with-current-financial-period.md)
- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-005 — Income Management Flow](005-income-management-flow.md)
- [SC-006 — Expense Management Flow](006-expense-management-flow.md)
- [SC-007 — Investment and Balance Flow](007-investment-and-balance-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)
