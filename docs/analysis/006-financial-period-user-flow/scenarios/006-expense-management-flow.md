# Scenario SC-006 — Expense Management Flow

## Purpose

Describe how the user reviews, creates, and enters expense information within an open financial period.

## Trigger

The user accesses the expense section from the financial period overview.

## Preconditions

- A financial period has been loaded.
- The expense section is available in the financial period overview.
- The financial period status is known.

## Main Flow

1. The system displays the expenses associated with the financial period.
2. For each expense, the system displays its description, type, planned amount, actual amount, and current state when required.
3. The system identifies expenses that contain details.
4. The user reviews planned and actual expense information.
5. If the period is open, the user may execute the expense operations supported for that expense type and state.
6. After a successful operation, the system refreshes the expense section.
7. The system recalculates and refreshes the financial summary, investment, and affected balances.

## Expense Types

The financial period may contain:

- Recurring expenses.
- Fixed-term expenses.
- Discretionary expenses.

The creation and entry operations available for each type must respect the existing domain and application capabilities.

## Alternative Flows

### AF-001 — Add a Discretionary Expense

1. The user starts the discretionary-expense operation.
2. The system requests the required expense information, including its date.
3. The user enters and confirms the information.
4. The system validates the input.
5. The system adds the discretionary expense to the financial period.
6. The system refreshes expense totals, investment, and balances.

### AF-002 — Add a Fixed-Term Expense

1. The user starts the fixed-term expense operation.
2. The system requests the information required by the fixed-term expense model.
3. The user enters and confirms the information.
4. The system validates the input.
5. The system adds the fixed-term expense to the financial period.
6. The system refreshes expense totals, investment, and balances.

The exact fields required by this operation must be verified against the existing command and domain model.

### AF-003 — Open an Expense With Details

1. The user selects an expense that contains details.
2. The system opens the expense-detail popup.
3. The popup displays the details currently associated with the expense.
4. If the period and expense state permit it, the user may enter the expense using its details.
5. The system validates and records the detail information.
6. The expense actual amount is recalculated from the sum of its details.
7. The popup and financial period overview display the updated values.

### AF-004 — Select an Expense Without Details

1. The system identifies that the expense has no details.
2. The system does not open the detail popup.
3. Any direct actual-amount entry operation depends on the backend capabilities confirmed during the API contract review.

### AF-005 — The Financial Period Is Closed

1. The system displays the expense information for consultation.
2. The user may review existing expense details.
3. The system does not allow expenses or details to be added or entered.

### AF-006 — No Expenses Exist

1. The system displays an empty expense state.
2. If the period is open, the system presents the supported expense-creation operations.
3. The system does not present the absence of expenses as an error.

### AF-007 — Input or Domain Validation Fails

1. The system rejects the invalid operation.
2. The system communicates the applicable validation or business-rule failure.
3. The expense and financial period remain unchanged.
4. The user may correct the information and retry.

### AF-008 — The Backend Operation Fails

1. The system informs the user that the expense operation could not be completed.
2. The interface does not present the attempted operation as successful.
3. The system preserves the last confirmed financial period state.

## Postconditions

- The expense section reflects the latest confirmed financial period state.
- Planned and actual expense amounts remain independently visible.
- An expense actual amount based on details equals the sum of those details.
- Financial totals, investment, and balances reflect successful expense operations.

## Business Rules

- Planned and actual expense amounts are independent values.
- When an expense uses details, its actual amount is calculated from the sum of those details.
- A discretionary expense requires a date.
- A closed financial period cannot be modified.
- Expense operations must comply with the rules associated with the expense type and state.
- An unexpected expense affects the actual investment according to the existing domain rules.
- Generating a following period resets the applicable actual expense amounts according to the existing domain rules.

## Backend Capability Requirements

Confirmed application capabilities from the existing MVP include:

- Add a discretionary expense.
- Add a fixed-term expense.
- Enter an expense using details.

The API contract review must verify:

- Retrieval of expenses and their details.
- Creation support for each expense type.
- Direct actual-amount entry for expenses without details.
- Required fields for fixed-term and discretionary expenses.
- Updated summary, investment, and balance retrieval after an operation.
- Error responses for nonexistent, closed, invalid, or unrelated entities.

## Open Questions

- Which expense types can be created manually in the presentation layer?
- How are recurring expenses introduced into the first financial period?
- Can planned expense information be edited after creation?
- Can an entered expense or expense detail be corrected?
- Does the detail popup support both consultation and data entry?
- How should the interface distinguish expenses with and without details?
- Is direct actual-amount entry required for expenses without details?

## Related Scenarios

- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-005 — Income Management Flow](005-income-management-flow.md)
- [SC-007 — Investment and Balance Flow](007-investment-and-balance-flow.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)
