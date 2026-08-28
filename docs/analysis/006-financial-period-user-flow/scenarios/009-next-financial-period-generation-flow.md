# Scenario SC-009 — Next Financial Period Generation Flow

## Purpose

Describe how Beridian generates a financial period from the immediately preceding period while preserving monthly continuity and applying the defined carry-forward rules.

## Trigger

The generation process starts in one of the following contexts:

- The user requests generation of the financial period following a selected period.
- Beridian detects missing periods while bringing the financial history up to date, as defined in SC-003.
- A scheduled background task runs on the first day of the month and requests generation of the current financial period.

The exact user-interface entry point for manual generation remains to be confirmed.

## Preconditions

- A source financial period exists.
- The month immediately following the source period can be determined.
- A financial period does not already exist for the target month.
- The source period contains the information required by the domain generation rules.

The source period may remain open because Beridian permits more than one open financial period at the same time.

## Main Flow

1. The generation process identifies the source financial period.
2. The system determines the immediately following month and year.
3. The system verifies that the target financial period does not already exist.
4. The system requests generation of the next financial period.
5. The domain creates the target period from the source period.
6. The domain applies the defined carry-forward and reset rules to incomes, expenses, investment, and balances.
7. The system stores the generated financial period.
8. The source financial period remains unchanged.
9. The system retrieves the generated period.
10. When the generated period is the period the user must work with, the system displays it.

## Generation Rules

The generated financial period must follow the domain rules already defined for period continuity, including:

- The target period is the month immediately following the source period.
- The source period remains unchanged.
- Actual values that must start over in a new period are reset according to their domain rules.
- Planned income is derived from the preceding period's actual income according to the existing domain rule.
- Applicable expenses are carried forward or completed according to their type and duration rules.
- Discretionary expense values that must restart are initialized according to their domain rules.
- Planned investment is recalculated according to the planned balance objective.
- Actual investment is left for the user's decision according to the existing domain rule.
- The remaining balance from the source period is represented as transferred balance in the generated period.

The exact property-level mapping must be verified against the domain model and the existing generation documentation before implementation.

## Alternative Flows

### AF-001 — The Target Financial Period Already Exists

1. The system detects an existing financial period for the target month.
2. The system does not generate a duplicate period.
3. The existing target period remains unchanged.
4. If appropriate for the originating flow, the system displays or returns the existing period.

### AF-002 — Several Financial Periods Are Missing

1. The system generates the oldest missing period from the latest preceding period.
2. The generated period becomes the source for the next missing month.
3. The system repeats the operation in chronological order.
4. The process continues until the current financial period exists.

The complete catch-up behavior is defined in SC-003.

### AF-003 — The Source Financial Period Is Open

1. The system does not close the source financial period automatically.
2. The system generates the following financial period using the applicable domain rules.
3. Both periods may remain open.

### AF-004 — Scheduled Monthly Generation

1. The scheduled background task starts on the first day of the month.
2. The task determines the current month and year.
3. The task verifies whether the current financial period already exists.
4. If the current period does not exist, the task retrieves the immediately
   preceding financial period.
5. The task requests generation of the current financial period.
6. The domain applies the carry-forward and reset rules.
7. The generated financial period is persisted.
8. The source financial period remains unchanged.
9. The scheduled task finishes successfully.
10. 
### AF-006 — Domain Validation Prevents Generation

1. The domain rejects the generation request.
2. The target financial period is not presented as successfully generated.
3. The system communicates the applicable business-rule failure.
4. The source financial period remains unchanged.

### AF-007 — Persistence or Backend Failure

1. The system informs the user that the next financial period could not be generated.
2. The interface does not present an unconfirmed target period as stored.
3. The system preserves the last confirmed financial history state.
4. The user or automatic synchronization process may retry the operation.

## Postconditions

- A financial period exists for the target month after successful generation.
- No duplicate financial period is created for the same month and year.
- The generated period follows the defined carry-forward and reset rules.
- The source period remains unchanged and retains its previous status.
- Monthly continuity is preserved.

## Business Rules

- The generated period must immediately follow its source period.
- Only one financial period may exist for a given month and year.
- Generating a period does not automatically close its source period.
- More than one financial period may remain open at the same time.
- Period generation must preserve the invariants of the `FinancialPeriod` aggregate.
- Missing periods are generated chronologically and cannot be skipped during automatic catch-up.
- Scheduled generation must not create a duplicate financial period.
- A failure of scheduled generation does not prevent later manual generation or automatic catch-up.

## Backend Capability Requirements

The API contract review must verify:

- Retrieval of a financial period by month and year.
- Detection of an existing target period.
- An application operation for generating the next financial period.
- Persistence of the complete generated aggregate.
- Retrieval of the generated period for presentation.
- Safe execution of consecutive generation operations.
- Error reporting that identifies the period that could not be generated.

No implemented next-period generation command or handler has yet been confirmed in the current MVP inventory.

## Open Questions

- From which interface action will the user manually generate the next period?
- Should generation be offered immediately after closing a period?
- Should Beridian generate the next period automatically after closing, or only when it is needed?
- What information should be shown before the user confirms manual generation?
- What summary of carried-forward values should be shown after generation?
- Should automatically generated periods be visually identified?
- Should multi-period catch-up be atomic or preserve each successfully generated period?

## Related Scenarios

- [SC-003 — Access With History but No Current Financial Period](003-access-with-history-but-no-current-financial-period.md)
- [SC-004 — Financial Period Overview](004-financial-period-overview.md)
- [SC-005 — Income Management Flow](005-income-management-flow.md)
- [SC-006 — Expense Management Flow](006-expense-management-flow.md)
- [SC-007 — Investment and Balance Flow](007-investment-and-balance-flow.md)
- [SC-008 — Financial Period Closing Flow](008-financial-period-closing-flow.md)
