# Scenario SC-004 — Financial Period Overview

## Purpose

Describe how Beridian presents the current financial period and provides the user with an immediate understanding of its planned and actual financial state.

## Trigger

Beridian successfully retrieves the financial period selected as the application entry point.

## Preconditions

- A financial period has been retrieved.
- The financial information required to present the period is available.
- The financial period may be open or closed.

## Main Flow

1. The system displays the financial period month, year, and status.
2. The system displays the financial summary as the first section.
3. The financial summary presents the balances and the planned and actual totals for the period.
4. Below the financial summary, the system displays expenses on the left and incomes on the right.
5. Each expense and income presents its planned and actual amounts.
6. The system identifies expenses that contain details.
7. Below the expense and income section, the system displays the investment section as a third row.
8. The investment section presents its planned and actual amounts.
9. The system enables only the operations permitted by the financial period status.

## Information Structure

The financial period overview is organized into three vertical sections.

### Financial Period Navigation

The financial period overview includes a navigation bar that allows the user to:

- Navigate to the immediately preceding financial period.
- Navigate to the immediately following financial period.
- Search for and select a specific financial period.
- Return to the current financial period.
- Identify the month, year, and status of the displayed period.

Selecting another period does not modify the current or selected financial period.

### First Section — Financial Summary

The financial summary provides the consolidated financial state of the period.

It includes:

- Opening or transferred balance.
- Planned income total.
- Actual income total.
- Planned expense total.
- Actual expense total.
- Planned investment amount.
- Actual investment amount.
- Planned remaining balance.
- Actual remaining balance.

The exact labels and visual representation will be defined during the screen and interaction design session.

### Second Section — Expenses and Incomes

The second section is divided into two columns:

- Expenses are displayed on the left.
- Incomes are displayed on the right.

Both columns present the planned and actual amounts associated with their records.

An expense that contains details provides an interaction for opening those details in a popup.

### Third Section — Investment

The investment section appears below the expense and income columns as a separate third row.

It presents:

- Planned investment amount.
- Actual investment amount.

The investment operations available to the user will be defined in the investment and balance scenario.

## Conceptual Layout

```text
┌─────────────────────────────────────────────────────────────┐
│                    FINANCIAL SUMMARY                        │
│                                                             │
│  Opening / Transferred Balance                              │
│  Planned Totals                     Actual Totals            │
│  Planned Remaining Balance          Actual Remaining Balance │
└─────────────────────────────────────────────────────────────┘

┌────────────────────────────────┬────────────────────────────┐
│            EXPENSES            │           INCOMES          │
│                                │                            │
│  Planned │ Actual              │  Planned │ Actual          │
│  Expense details → popup       │                            │
└────────────────────────────────┴────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                         INVESTMENT                          │
│                                                             │
│             Planned Amount │ Actual Amount                  │
└─────────────────────────────────────────────────────────────┘
```

This layout describes information placement only. It does not define the final visual design or Angular component structure.

## Alternative Flows

### AF-001 — The Financial Period Is Closed

1. The system displays the same financial summary and period content.
2. The system identifies the period as closed.
3. The information remains available for consultation.
4. Operations that modify the period are unavailable.

### AF-002 — The User Selects an Expense With Details

1. The user selects an expense that contains details.
2. The system opens a popup containing the expense details.
3. The user reviews the information associated with the expense.
4. The user closes the popup and returns to the financial period overview.

Whether the popup also permits adding or modifying details will be defined in SC-006.

### AF-003 — The User Selects an Expense Without Details

1. The system identifies that the selected expense has no details.
2. The system does not open the expense-detail popup.
3. Any operation available for the expense is determined by the expense management scenario.

### AF-004 — The Financial Period Cannot Be Loaded Completely

1. The system detects that the financial period information could not be retrieved completely.
2. The system does not present incomplete totals as valid financial information.
3. The system informs the user that the financial period overview could not be loaded.
4. The system provides an option to retry the operation.

### AF-005 — Navigate to Another Financial Period

1. The user selects the previous period, next period, or a specific period.
2. The system determines the requested month and year.
3. The system retrieves the selected financial period.
4. Beridian displays its financial summary, expenses, incomes, investment, and status.
5. If the selected period is closed, only consultation operations are available.
6. If the selected period is open, operations permitted by its state remain available.


## Postconditions

- The user can identify the financial period and its status.
- The user can understand the planned and actual financial state of the period.
- The user can review expenses, incomes, investment, and balances from a single overview.
- The user can access the details of an expense when they exist.
- The available operations are consistent with the financial period status.

## Business Rules

- Planned and actual values remain independently visible.
- A closed financial period is available for consultation but cannot be modified.
- An expense actual amount derived from details must remain consistent with the sum of those details.
- The financial summary must be calculated from the financial period information.
- The overview must not modify the financial period merely by displaying it.

## Backend Capability Requirements

- Retrieve a financial period by its identifier or month and year.
- Retrieve the period status.
- Retrieve opening and transferred balance information.
- Retrieve planned and actual totals.
- Retrieve incomes and their planned and actual amounts.
- Retrieve expenses and their planned and actual amounts.
- Identify whether an expense contains details.
- Retrieve investment planned and actual amounts.
- Retrieve the information required to calculate or present remaining balances.
- Retrieve the immediately preceding financial period.
- Retrieve the immediately following financial period.
- Search for a financial period by month and year.
- Return to the financial period corresponding to the current month.

Backend support and the final response model must be verified during the API contract review.


## Future Enhancements

The financial period overview follows a monthly financial statement model,
prioritizing balances and financial movements over analytical visualizations.

Future versions may complement this operational view with:

- Balance projections.
- Planned versus actual trends.
- Expense distribution charts.
- Investment evolution.
- Financial period comparisons.

Charts will complement the financial statement and will not replace the
underlying income, expense, investment, and balance information.

## Open Questions

- Which balances should receive the highest visual priority in the summary?
- Should expense and income records display their current domain status?
- Should previous open financial periods be indicated from this overview?
- Which actions should be available directly from each section?
- Should the expense-detail popup support consultation only or also data entry?
- How should empty income, expense, or investment sections be represented?

## Related Scenarios

- [SC-001 — First Access Without Financial History](001-first-access-without-financial-history.md)
- [SC-002 — Access With a Current Financial Period](002-access-with-current-financial-period.md)
- [SC-003 — Access With History but No Current Financial Period](003-access-with-history-but-no-current-financial-period.md)
- [SC-005 — Income Management Flow](005-income-management-flow.md)
- [SC-006 — Expense Management Flow](006-expense-management-flow.md)
- [SC-007 — Investment and Balance Flow](007-investment-and-balance-flow.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
