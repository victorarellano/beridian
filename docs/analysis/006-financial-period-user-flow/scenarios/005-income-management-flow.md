# Scenario SC-005 — Income Management Flow

## Purpose

Describe how the user reviews, adds, and enters income information within an open financial period.

## Trigger

The user accesses the income section from the financial period overview.

## Preconditions

- A financial period has been loaded.
- The income section is available in the financial period overview.
- The financial period status is known.

## Main Flow

1. The system displays the incomes associated with the financial period.
2. For each income, the system displays its description, planned amount, and actual amount.
3. The user reviews the difference between planned and actual income.
4. If the period is open, the user may add a new income or enter the actual amount of an existing income.
5. After a successful operation, the system refreshes the income section.
6. The system recalculates and refreshes the financial summary and affected balances.
7. The updated financial period remains displayed.

## Alternative Flows

### AF-001 — Add a New Income

1. The user starts the add-income operation.
2. The system requests the income description and planned amount.
3. The user enters the required information.
4. The system validates the input.
5. The system adds the income to the financial period.
6. The new income appears in the income section with no actual amount entered.
7. The system refreshes planned totals and balances.

### AF-002 — Enter an Actual Income Amount

1. The user selects an income that has not yet received its actual amount.
2. The system requests the actual amount received.
3. The user enters and confirms the amount.
4. The system validates the operation.
5. The system records the actual income amount.
6. The system refreshes actual totals and balances.

The actual amount may differ from the planned amount.

### AF-003 — The Financial Period Is Closed

1. The system displays the income information for consultation.
2. The system does not allow incomes to be added or entered.
3. The planned and actual amounts remain visible.

### AF-004 — No Incomes Exist

1. The system displays an empty income state.
2. If the period is open, the system makes the add-income operation available.
3. The system does not present missing income records as an error.

### AF-005 — Input Validation Fails

1. The system rejects the invalid input.
2. The system identifies the field or condition that must be corrected.
3. The income is not added or entered.
4. The financial period remains unchanged.

### AF-006 — The Backend Operation Fails

1. The system informs the user that the income operation could not be completed.
2. The interface does not present the attempted value as successfully stored.
3. The system preserves the last confirmed financial period state.
4. The user may retry the operation.

## Postconditions

- The income section reflects the latest confirmed financial period state.
- Planned and actual income amounts remain independently visible.
- Financial totals and balances reflect successful income operations.
- A failed operation does not appear as successfully completed.

## Business Rules

- Planned and actual income amounts are independent values.
- Entering an actual income does not replace its planned amount.
- The actual income amount may differ from the planned amount.
- A closed financial period cannot be modified.
- When a following financial period is generated, its planned income is derived from the preceding period's actual income according to the existing domain rules.
- The actual income amount of a newly generated period starts at zero.

## Backend Capability Requirements

Confirmed application capabilities from the existing MVP include:

- Add an income to a financial period.
- Enter the actual amount of an existing income.

The API contract review must verify:

- Retrieval of the income list.
- Required fields and validation responses.
- Updated financial summary retrieval after a successful operation.
- Error responses for nonexistent, closed, or invalid financial periods.

## Open Questions

- Should income creation and actual entry use inline controls, a popup, or a separate form?
- Can an income description or planned amount be edited after creation?
- Can an entered actual income amount be corrected?
- Should the interface display an explicit income status?
- Which validation messages should be handled directly by the frontend?

## Related Scenarios

- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-007 — Investment and Balance Flow](007-investment-and-balance-flow.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)
