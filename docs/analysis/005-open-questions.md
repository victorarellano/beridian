# Open Questions

## Purpose

This document records business questions that remain unanswered after the current domain discovery process.

Unlike business rules, these questions do not yet have an agreed business answer.

Their purpose is to ensure that unresolved topics remain visible and can be revisited during future analysis sessions.

Once a question has been answered, it should either:

- become a Business Rule;
- become a Domain Assumption;
- or be removed if no longer relevant.

---

# Financial Period

### OQ-001 — Can a Closed financial period be reopened?

Current analysis assumes that a financial period becomes definitive after being closed.

However, the business has not yet defined whether reopening a closed period should be allowed.

---

### OQ-002 — What should happen if historical financial information must be corrected?

The current process assumes that financial information is complete before closing a financial period.

The business has not yet defined how corrections should be handled after closure.

---

# Expenses

### OQ-003 — Should every expense category remain configurable?

The current analysis identifies three business behaviors:

- recurring;
- fixed-term;
- discretionary.

It has not yet been decided whether additional behaviors may be introduced in the future.

---

### OQ-004 — Should expense categories define their own carry-forward strategy?

Current analysis recognizes that each expense type behaves differently.

The business has not yet defined whether this behavior should be configurable or fixed.

---

# Investment

### OQ-005 — Should investment support multiple investment targets?

Current analysis treats investment as a single financial concept.

The business has not yet determined whether future financial planning should support multiple investment objectives.

---

# Financial Analysis

### OQ-006 — Should historical financial metrics become part of the business domain?

Current analysis focuses exclusively on financial planning and execution.

Historical analysis, trends, projections, and financial indicators have not yet been explored.

---

# Future Evolution

### OQ-007 — Are additional financial concepts required?

Current analysis identified:

- FinancialPeriod;
- Expense;
- ExpenseDetail;
- Income;
- Investment.

Future analysis may reveal additional domain concepts that are currently unknown.

---

# Document Maintenance

Open questions should be reviewed periodically.

Each resolved question should be moved to its appropriate documentation:

- Business Rules;
- Domain Assumptions;
- Domain Model;
- or another domain document if appropriate.