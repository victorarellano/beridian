# Business Rules

## Purpose

This document defines the business rules identified during the domain discovery process.

Business rules describe the constraints, behaviors, and decisions that govern the financial management process.

They are independent of any software implementation and represent the official business policy of the Beridian domain.

---

# Financial Period

### BR-001 — The business is organized into monthly financial periods.

Every financial activity belongs to one and only one financial period.

---

### BR-002 — Every financial period starts in the Open state.

A newly created financial period is immediately available for financial planning and execution.

---

### BR-003 — A financial period is closed manually.

The user decides when a financial period is complete.

Closing is allowed only after the user determines that all income and expense information has been entered.

When closed, the financial period transitions to the **Closed** state.

---

### BR-004 — Only Closed financial periods produce definitive balances.

The remaining balance becomes definitive only after the financial period reaches the **Closed** state.

---

### BR-005 — Financial period creation policy.

A future financial period may be created even if the previous financial period has not yet been closed.

---

### BR-006 — The transferred balance may be provisional.

If the next financial period is created before the previous one is closed, the transferred balance is considered provisional until the previous period reaches its final state.

---

### BR-007 — The remaining balance is transferred to the next financial period.

Once a financial period is closed, its remaining balance becomes the opening balance of the following financial period.

---

# Expenses

### BR-008 — Every expense has a planned amount.

Planning is mandatory for every expense.

---

### BR-009 — The actual amount starts at zero.

Actual values are recorded during the execution of the financial period.

---

### BR-010 — Expense details are optional.

Some expenses require detailed financial movements.

Others require only a single monthly amount.

---

### BR-011 — When expense details exist, the actual amount is calculated from the details.

The actual amount cannot be entered directly while detail records exist.

---

### BR-012 — Planned amounts remain independent from detail records.

Planning is performed at the expense level.

Expense details are used only to record actual execution.

---

### BR-013 — Expenses are classified according to business behavior.

The supported categories are:

- recurring;
- fixed-term;
- discretionary.

Each category follows different carry-forward rules.

---

### BR-014 — An expense progresses through business states.

Every expense starts in the Created state.

After the required financial information has been entered, it transitions to the Entered state.

---

# Income

### BR-015 — Income is managed without detail records.

Income consists only of planned and actual values.

---

### BR-016 — Future planning uses the previous actual income.

When preparing a new financial period, the planned income is initialized from the actual income of the previous financial period.

---

# Investment

### BR-017 — Planned investment is calculated during planning.

Its purpose is to balance the financial plan before execution begins.

---

### BR-018 — Actual investment depends on financial execution.

Actual investment changes as actual income and actual expenses replace planned values.

It may become greater than, equal to, or smaller than the planned investment.

---

# Period Generation

### BR-019 — A new financial period starts from the previous one.

Business information is carried forward according to the rules of each financial concept.

---

### BR-020 — Carry-forward behavior depends on the business concept.

Different financial concepts may follow different carry-forward rules.

For example:

- recurring expenses continue;
- fixed-term expenses continue until completed;
- discretionary expenses are reinitialized;
- previous balances are transferred.

---

# Consistency

### BR-021 — Planning and execution are independent.

Planning defines expectations.

Execution records reality.

Both coexist until the financial period is closed.

---

### BR-022 — Business consistency is achieved when the financial period is closed.

The financial result becomes definitive only after the closing process has been completed.

---

### BR-023 — A financial period progresses through business states.

Every financial period starts in the Open state.

Once the user confirms that all financial information has been entered, the period transitions to the Closed state.


## Matrice Domain Design.

| Rule   | Business Rule                            | Category         | Domain Responsibility    | Domain Enforcement                                                                          |
| ------ | ---------------------------------------- | ---------------- | ------------------------ | ------------------------------------------------------------------------------------------  |
| BR-001 | Monthly financial periods                | Business Process | FinancialPeriod          | Must be created for one valid monthly Period and owns the financial concepts of that period |
| BR-002 | Initial Open state                       | Lifecycle        | FinancialPeriod          | A new FinancialPeriod is always initialized in the Open state and cannot exist without a valid state. |
| BR-003 | Manual period closure                    | Lifecycle        | FinancialPeriod          | The period can be closed only when all Expenses and Incomes have been entered.              |
| BR-004 | Definitive balances                      | Consistency      | FinancialPeriod          | The remaining balance is considered definitive only when the FinancialPeriod is in the Closed state. |
| BR-005 | Financial period creation policy         | Business Process | FinancialPeriodGenerator | A new FinancialPeriod may be created even if the previous FinancialPeriod is still Open. |
| BR-006 | Transferred Balance                      | Carry-Forward    | FinancialPeriodGenerator | When the source FinancialPeriod is Open, its transferred balance is treated as provisional until that period is closed. |
| BR-007 | Remaining balance transfer               | Carry-Forward    | FinancialPeriodGenerator | When the source FinancialPeriod is closed, its remaining balance becomes the opening balance of the following FinancialPeriod. |
| BR-008 | Planned expense amount                   | Planning         | Expense               | 
| BR-009 | Actual amount starts at zero             | Execution        | Expense               | 
| BR-010 | Optional expense details                 | Structure        | Expense               | 
| BR-011 | Actual calculated from details           | Calculation      | Expense               | The actual amount is always calculated from the sum of its ExpenseDetails. |
| BR-012 | Planned amount independent from details  | Planning         | Expense               | The planned amount remains independent from the existence or value of ExpenseDetails. |
| BR-013 | Expense classification                   | Classification   | Expense               | Every Expense is classified according to its business behavior and the classification determines its carry-forward policy. |
| BR-014 | Expense lifecycle                        | Lifecycle        | Expense               | An Expense is created in the Created state and changes to Entered only through its defined domain behavior. When details exist, registering a detail does not automatically imply that the parent Expense has completed its lifecycle transition. |
| BR-015 | Income without details                   | Structure        | Income                | Income manages its planned and actual amounts directly and does not allow detail records to be associated with it. |
| BR-016 | Planning based on previous actual income | Planning         | Income                | When an Income is carried forward, its previous actual amount becomes the planned amount of the newly generated Income, while the new actual amount starts at zero. |
| BR-017 | Planned investment calculation           | Planning         | Investment            | Investment calculates its planned amount from the planned financial distribution of the FinancialPeriod, allocating the planned remaining amount according to the investment rule. |
| BR-018 | Actual investment depends on execution   | Execution        | Investment            | The actual investment amount is explicitly determined by the user from the available financial execution and is not automatically derived from the planned investment amount. |
| BR-019 | New financial period generation          | Carry-Forward    | FinancialPeriodGenerator | The next FinancialPeriod is generated using the previous FinancialPeriod as its source. |
| BR-020 | Carry-forward by business concept        | Carry-Forward    | FinancialPeriodGenerator Income Expense Investment | The generator coordinates the operation, while each financial concept applies its own carry-forward rule. |
| BR-021 | Planning and execution independence      | Business Process | FinancialPeriod       | Planned and actual amounts are maintained as separate values; changes in financial execution do not overwrite the corresponding planned values. |
| BR-022 | Business consistency on period closure   | Consistency      | FinancialPeriod       | FinancialPeriod validates that all required Expense and Income records have been entered before allowing the manual transition to Closed. |
| BR-023 | Financial period lifecycle               | Lifecycle        | FinancialPeriod       | FinancialPeriod is created in the Open state and can transition only from Open to Closed; reopening or modifying a closed period is not allowed. |

