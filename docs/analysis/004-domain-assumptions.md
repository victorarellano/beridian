# Domain Assumptions

## Purpose

This document records the assumptions made during the domain analysis of the Beridian project.

Unlike business rules, these assumptions are not considered immutable truths of the business.

They represent the current understanding of the domain and may evolve as additional business knowledge is acquired.

Their purpose is to make analysis decisions explicit so they can be validated or revised in future iterations.

---

# Assumptions About the Domain

### DA-001 — FinancialPeriod is assumed to be the central business concept.

Current analysis indicates that every relevant financial activity occurs within a financial period.

For this reason, FinancialPeriod is considered the natural center of the domain.

This assumption will be validated during domain modeling.

---

### DA-002 — FinancialPeriod is assumed to become the Aggregate Root.

Current analysis suggests that FinancialPeriod will own the consistency boundary of the financial domain.

This architectural consequence has not yet been formally designed.

---

### DA-003 — Expense behavior is more important than expense structure.

Expenses are primarily distinguished by their business behavior rather than by their stored data.

Future modeling should preserve this characteristic.

---

### DA-004 — ExpenseDetail exists only when required by the business.

Expense details are assumed to be optional.

No business evidence currently indicates that every expense should require detailed transactions.

---

### DA-005 — Planning and execution represent different business concepts.

Planning is treated as an expected financial scenario.

Execution represents actual financial activity.

Although both coexist within the same financial period, they serve different business purposes.

---

### DA-006 — Financial continuity is a core characteristic of the domain.

Financial periods are assumed to be connected through business continuity rather than existing as isolated monthly snapshots.

---

### DA-007 — Carry-forward behavior belongs to the business.

The logic used when creating a new financial period is considered business knowledge rather than technical implementation.

Future software design should preserve this responsibility inside the domain.

---

### DA-008 — Manual business decisions remain part of the domain.

Some business activities intentionally require user decisions.

Examples include:

- closing a financial period;
- deciding when discretionary expenses are complete;
- validating financial information before closing.

The current analysis assumes these decisions should remain explicit business operations.

---

### DA-009 — Business concepts should drive software design.

Classes, database tables, and APIs should emerge from the domain model instead of defining it.

This assumption guides all future design activities.

---

# Validation

Every assumption documented here should eventually become one of the following:

- validated;
- refined;
- rejected.

This document is therefore expected to evolve as the project progresses.

---

# Relationship with Other Documents

This document complements:

- Current Business Process
- Domain Discovery
- Business Rules

It should be reviewed before making architectural or implementation decisions to ensure that software continues to reflect the current understanding of the business domain.