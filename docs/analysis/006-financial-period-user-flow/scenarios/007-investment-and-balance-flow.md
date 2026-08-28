# Scenario SC-007 — Investment and Balance Flow

## Purpose

Describe how Beridian presents and maintains the relationship between planned investment, actual investment, and the financial period balances.

## Trigger

The user reviews the financial summary or accesses the investment section of the financial period overview.

## Preconditions

- A financial period has been loaded.
- Income and expense information required for the financial calculations is available.
- The financial period status is known.

## Main Flow

1. The system displays the balances in the financial summary.
2. The system displays the investment section below the expense and income sections.
3. The system presents the planned investment amount and actual investment amount independently.
4. The user reviews how planned income, planned expenses, and the available planned balance relate to planned investment.
5. The user reviews how actual income, actual expenses, and the available actual balance relate to actual investment.
6. When a confirmed income or expense operation changes the financial state, the system refreshes the investment and balance information.
7. If the period is open, the system makes available the investment operations supported by the backend.

## Balance Information

The financial period overview may present:

- Opening or transferred balance.
- Planned remaining balance.
- Actual remaining balance.
- Planned investment amount.
- Actual investment amount.

The exact balance labels and calculation ownership must be verified against the domain model and API response contract.

## Alternative Flows

### AF-001 — Planned Financial Information Changes

1. A planned income or planned expense is added or changed through a supported operation.
2. The system obtains the updated planned financial state.
3. The planned investment reflects the amount required by the domain model to maintain the planned balance objective.
4. The system refreshes the planned investment and planned balance.

### AF-002 — Actual Income or Expense Is Entered

1. The user records an actual income or expense operation.
2. The system obtains the updated actual financial state.
3. The system refreshes the actual remaining balance.
4. The system refreshes the actual investment when the domain rules require an adjustment.

### AF-003 — An Unexpected Expense Is Added

1. The user adds an unexpected expense to the open financial period.
2. The system validates and records the expense.
3. The domain reduces the actual investment according to the applicable rule.
4. The system displays the updated actual investment and actual balance.

### AF-004 — The User Defines the Actual Investment

1. The user starts the actual-investment operation.
2. The system requests the amount decided by the user.
3. The user enters and confirms the amount.
4. The system validates and records the actual investment.
5. The system refreshes the actual investment and remaining balance.

This flow depends on an application and API capability that must be verified or implemented.

### AF-005 — The Financial Period Is Closed

1. The system displays planned and actual investment and balance information for consultation.
2. The system does not allow the investment information to be modified.
3. Existing values remain visible as part of the financial statement.

### AF-006 — Investment or Balance Information Cannot Be Retrieved

1. The system does not present incomplete values as a valid financial state.
2. The system informs the user that the investment and balance information could not be loaded.
3. The system provides an option to retry the operation.

## Postconditions

- Planned and actual investment remain independently visible.
- Balances reflect the latest confirmed financial period state.
- Successful income and expense operations are reflected in the related investment and balance values.
- A closed period remains available for consultation without modification.

## Business Rules

- Planned investment is used to maintain a planned remaining balance of zero according to the existing domain model.
- Actual investment is decided by the user.
- Planned and actual investment are independent values.
- An unexpected expense reduces the actual investment according to the existing domain rule.
- Remaining balance from a previous period is represented through the transferred balance of the following period.
- A closed financial period cannot be modified.

## Backend Capability Requirements

The API contract review must verify:

- Retrieval of planned and actual investment amounts.
- Retrieval of opening and transferred balances.
- Retrieval or calculation of planned and actual remaining balances.
- Automatic recalculation after income and expense operations.
- An application operation for entering or changing actual investment.
- Consistent response data after an unexpected expense affects investment.

No implemented investment command or handler has yet been confirmed in the current MVP inventory.

## Open Questions

- Which balance values are calculated by the domain and which are calculated only for presentation?
- When and how does the user enter the actual investment amount?
- Can the actual investment be corrected after entry?
- What happens if an unexpected expense is greater than the current actual investment?
- Should investment adjustments caused by unexpected expenses be explained in the interface?
- Should balance and investment changes retain an audit trail?
- Which balance should receive the greatest visual emphasis in the financial summary?

## Future Enhancements

Future analytical views may complement the financial statement with:

- Investment evolution across financial periods.
- Planned versus actual investment trends.
- Balance projections.
- Comparisons between financial periods.

These visualizations will complement rather than replace the underlying financial statement information.

## Related Scenarios

- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-005 — Income Management Flow](005-income-management-flow.md)
- [SC-006 — Expense Management Flow](006-expense-management-flow.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
- [SC-009 — Next Financial Period Generation Flow](009-next-financial-period-generation-flow.md)
