# Current Business Process

## Purpose

This document describes the current business process used to manage personal finances before the Beridian application is implemented.

Its purpose is to understand how the business operates today, independently of any software implementation, database design, or architectural decisions.

The current business process serves as the primary source for discovering business concepts, business rules, and domain behavior.

---

# Scope

This document focuses exclusively on the business process.

It intentionally excludes:

- software architecture;
- object-oriented design;
- persistence models;
- APIs;
- implementation details.

Those topics are documented separately during later stages of the project.

---

# Business Objective

The monthly financial process aims to:

- plan expected income;
- plan expected expenses;
- estimate the amount available for investment;
- register actual financial movements;
- monitor financial execution throughout the month;
- determine the remaining balance;
- transfer the final balance to the following financial period.

The process is repeated for every financial period.

---

# Financial Period

The business is organized around monthly financial periods.

Each financial period represents a complete budgeting cycle containing:

- planned income;
- actual income;
- planned expenses;
- actual expenses;
- planned investment;
- actual investment;
- remaining balance.

Unlike a traditional monthly budget, multiple financial periods may coexist simultaneously.

A new financial period may already exist while the previous one is still being completed.

This allows financial planning to continue without waiting for the previous period to be formally closed.

As a consequence, a financial period may temporarily contain a provisional opening balance that is replaced by the final balance once the previous period has been closed.

---

# Financial Planning

At the beginning of a financial period, the expected financial plan is prepared.

Planning includes:

- expected income;
- expected expenses;
- expected investment.

The purpose of planning is to define an expected financial scenario before any real financial movement occurs.

The planned investment represents the amount expected to remain available after considering all planned income and planned expenses.

---

# Expense Management

Expenses are managed according to their business behavior.

Some expenses consist of a single monthly amount.

Other expenses require multiple financial movements before they are considered complete.

During the financial period, actual values gradually replace planned values as expenses are registered.

The business distinguishes three expense behaviors:

- recurring expenses;
- fixed-term expenses;
- discretionary expenses.

Each category follows its own business rules when a new financial period is created.

---

# Income Management

Income is managed as summarized monthly values.

Each income contains:

- planned amount;
- actual amount.

Income does not require individual transaction records.

Once the actual amount becomes known, it becomes the reference used when planning future financial periods.

---

# Investment Management

Investment represents the amount of money effectively preserved after all financial activity has been considered.

During planning, an expected investment amount is calculated from the planned financial scenario.

Throughout the month, actual income and actual expenses gradually modify that expectation.

As a result, the actual investment may become:

- greater than planned;
- equal to planned;
- smaller than planned.

The final investment amount cannot be determined until the financial period has been completed.

---

# Financial Execution

During the month, financial information is continuously updated.

Planned values are progressively replaced by actual values as financial movements occur.

Different business concepts require different registration processes.

Some are completed with a single update.

Others require multiple registrations before they are considered complete.

The financial period remains open until the user decides that every financial movement has been completely registered.

---

# Closing a Financial Period

Closing a financial period is a manual business activity.

It is independent of the calendar.

For example, a financial period may remain open even after the following month has already begun if pending financial movements still need to be registered.

Once the user determines that the financial information is complete, the period is closed and its financial result becomes definitive.

The remaining balance is then transferred to the following financial period.

---

# Preparing the Next Financial Period

A new financial period may be created as soon as the new month begins.

Its creation does not depend on the previous financial period being closed.

Initially, the new period uses the best financial information available at that moment.

If the previous period is closed later, the transferred balance is updated to reflect the definitive financial result.

This mechanism allows financial planning and financial execution to progress independently while maintaining business consistency.

---

# Current Business Challenges

The current business process relies heavily on manual work.

Business rules exist mainly as operational knowledge rather than explicit documentation.

Financial consistency depends on manual verification.

Creating a new financial period requires repetitive manual activities.

Carry-forward operations depend on user experience.

The process provides no automatic validation capable of detecting inconsistent financial information.

These limitations motivate the development of the Beridian application.