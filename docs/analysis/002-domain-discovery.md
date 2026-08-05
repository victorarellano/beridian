# Domain Discovery

## Purpose

This document describes the domain discovery process performed during the analysis of the Beridian project.

Its purpose is to capture the reasoning that transformed the current business process into a conceptual business domain.

The objective was not to design software, but to understand the business concepts, responsibilities, and rules that would later guide the implementation.

---

# Starting Point

The analysis began with the current manual financial management process.

At that stage, the available information consisted primarily of:

- the monthly financial workflow;
- the budgeting spreadsheet;
- the operational knowledge used to maintain it.

No assumptions were made about classes, databases, APIs, or software architecture.

The objective was simply to answer one question:

> **What is the business actually doing?**

---

# From Activities to Business Concepts

The first observation was that the spreadsheet itself was not the domain.

Instead, it was only a tool used to execute the business process.

The analysis therefore shifted from spreadsheet structures to business concepts.

For example:

| Spreadsheet View | Domain View |
|------------------|-------------|
| Expense rows | Expenses |
| Income rows | Incomes |
| Investment cell | Investment |
| Monthly worksheet | Financial Period |

This shift became the foundation for the remaining analysis.

---

# Identifying Business Responsibilities

Once the main concepts had been identified, the next step was to understand their responsibilities.

Instead of asking:

- What properties should this object contain?

The analysis focused on:

- What responsibility does this concept have?
- Which business rules belong to it?
- Which information does it own?
- Which operations can modify its state?

This prevented technical decisions from influencing the business model too early.

---

# Discovering the Central Business Concept

As the analysis progressed, it became evident that every important business operation occurred within the context of a financial period.

Expenses belonged to a financial period.

Income belonged to a financial period.

Investment belonged to a financial period.

Balances belonged to a financial period.

Period generation depended on a financial period.

Period closing depended on a financial period.

This observation naturally identified the FinancialPeriod as the central business concept around which the entire domain is organized.

At this stage, FinancialPeriod was recognized as the natural Aggregate Root candidate.

---

# Understanding Expense Behavior

The initial assumption was that every expense behaved similarly.

Further analysis showed that this was not true.

Three distinct business behaviors emerged:

- recurring expenses;
- fixed-term expenses;
- discretionary expenses.

The distinction was not based on data structure but on business behavior.

Each category follows different rules when creating a new financial period.

This became one of the first examples where business behavior proved to be more important than technical implementation.

---

# Discovering Optional Details

Another important discovery concerned expense details.

Initially, it appeared that every expense could contain detail records.

However, analysis revealed two different situations.

Some expenses require only a single monthly value.

Others require multiple financial movements before they are complete.

As a result, expense details became an optional business concept rather than a mandatory one.

This also established an important rule:

When details exist, the actual amount is obtained from those details instead of being entered directly.

---

# Discovering Business Lifecycles

The analysis revealed that several business concepts evolve through well-defined business lifecycles.

Rather than representing static data, these concepts progress through different business states as financial activities are completed.

### Expense Lifecycle

Every expense begins in the **Created** state.

Once the required financial information has been entered, the expense transitions to the **Entered** state.

This lifecycle reflects the completion of the business activity rather than any user interface interaction.

### Income Lifecycle

Income follows the same business lifecycle.

Every income starts in the **Created** state.

After its actual value has been registered, it transitions to the **Entered** state.

### Financial Period Lifecycle

A financial period follows a different lifecycle because it represents the execution of an entire financial cycle.

Every financial period begins in the **Open** state.

While open, financial information may continue to change as income and expenses are recorded.

The financial period transitions to the **Closed** state only after the user manually confirms that all financial information has been entered.

Once closed, the remaining balance becomes definitive and may be transferred to the following financial period.

This discovery established that business lifecycles are fundamental characteristics of the domain and should be modeled explicitly rather than inferred from data.

---

# Understanding Financial Continuity

One of the most important discoveries involved the relationship between consecutive financial periods.

Initially, each month appeared to be independent.

The analysis demonstrated the opposite.

Financial periods are connected through the remaining balance.

This created several additional discoveries.

A new financial period may exist before the previous one has been closed.

The transferred balance may therefore be provisional.

Once the previous period is finally closed, the transferred balance is updated with its definitive value.

This mechanism allows planning and execution to progress independently while preserving financial consistency.

---

# Discovering Investment Behavior

Investment was initially viewed as a simple remaining amount.

Further analysis showed that it represents the financial result of the entire planning process.

The planned investment is calculated from planned income and planned expenses.

The actual investment evolves during the month as actual financial movements replace planned values.

Consequently, the actual investment may become higher or lower than originally planned.

---

# Domain Knowledge Obtained

The analysis produced the following domain concepts:

- FinancialPeriod
- Expense
- ExpenseDetail
- Income
- Investment

It also identified:

- business lifecycles;
- financial continuity;
- carry-forward behavior;
- period closing behavior;
- period generation behavior;
- business consistency boundaries.

---

# Decisions Explicitly Deferred

The following topics were intentionally excluded from this analysis:

- class design;
- inheritance;
- composition;
- database schema;
- Entity Framework;
- repository interfaces;
- APIs;
- design patterns.

Those decisions belong to later design activities and should be based on the domain knowledge documented here.

---

# Outcome

The domain discovery process transformed a manual financial management process into a conceptual business model.

Instead of describing how information is stored, the resulting model describes:

- business concepts;
- business behavior;
- business responsibilities;
- business rules;
- business relationships.

This conceptual model becomes the foundation for the subsequent Domain Modeling phase.