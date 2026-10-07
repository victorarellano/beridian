# API Endpoint Inventory

## Purpose

This document records the API endpoints currently implemented by Beridian.
The inventory establishes the backend API capabilities available before mapping them to the Financial Period User Flows.
This document describes the current implementation only. It does not determine whether the existing API fully supports the designed user experience. That analysis is performed separately during the User Flow to API mapping and  backend gap analysis.

---

## API Structure

The Financial Period API is implemented using ASP.NET Core Minimal APIs.
Current API version:

`v1`

Base route:

`/api/v{version}/financial-periods`

The Financial Period endpoint group currently exposes operations for:
- Financial period lifecycle.
- Income management.
- Expense management.
- Expense detail management.
- Investment management.

---

## Endpoint Inventory

### Financial Period Lifecycle

| Operation | Method | Route | Request | Result | Application Operation |
|---|---|---|---|---|---|
| Create Financial Period | POST | `/api/v1/financial-periods/` | `CreateFinancialPeriodRequest` | `CreateFinancialPeriodResult` | `CreateFinancialPeriodCommand` |
| Get Financial Period | GET | `/api/v1/financial-periods/{financialPeriodId}` | Route: `financialPeriodId` | `GetFinancialPeriodResult` | `GetFinancialPeriodQuery` |
| Close Financial Period | POST | `/api/v1/financial-periods/{financialPeriodId}/close` | Route: `financialPeriodId` | `CloseFinancialPeriodResult` | `CloseFinancialPeriodCommand` |
| Generate Next Financial Period | POST | `/api/v1/financial-periods/{financialPeriodId}/next` | Route: `financialPeriodId` | `GenerateNextFinancialPeriodResult` | `GenerateNextFinancialPeriodCommand` |
| Synchronize Current Financial Period | POST | `/api/v1/financial-periods/synchronize` | `SynchronizeCurrentFinancialPeriodRequest` | `SynchronizeCurrentFinancialPeriodResult` | `SynchronizeCurrentFinancialPeriodCommand` |
| Get Financial Period By Period | GET | `/api/v1/financial-periods/by-period?year={year}&month={month}` | Query: `year`, `month` | `GetFinancialPeriodResult` | `GetFinancialPeriodByPeriodQuery` |

---

### Income Management

| Operation | Method | Route | Request | Result | Application Operation |
|---|---|---|---|---|---|
| Add Income | POST | `/api/v1/financial-periods/{financialPeriodId}/incomes` | `AddIncomeRequest` | `AddIncomeResult` | `AddIncomeCommand` |
| Enter Income | POST | `/api/v1/financial-periods/{financialPeriodId}/incomes/{incomeId}/entry` | `EnterIncomeRequest` | `EnterIncomeResult` | `EnterIncomeCommand` |

---

### Expense Management

| Operation | Method | Route | Request | Result | Application Operation |
|---|---|---|---|---|---|
| Add Recurring Expense | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/recurring` | `AddRecurringExpenseRequest` | `AddRecurringExpenseResult` | `AddRecurringExpenseCommand` |
| Add Fixed-Term Expense | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/fixed-term` | `AddFixedTermExpenseRequest` | `AddFixedTermExpenseResult` | `AddFixedTermExpenseCommand` |
| Add Discretionary Expense | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/discretionary` | `AddDiscretionaryExpenseRequest` | `AddDiscretionaryExpenseResult` | `AddDiscretionaryExpenseCommand` |
| Enter Expense | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry` | `EnterExpenseRequest` | `EnterExpenseResult` | `EnterExpenseCommand` |
| Enter Expense Using Details | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry-from-details` | Route IDs only | `EnterExpenseUsingDetailsResult` | `EnterExpenseUsingDetailsCommand` |

---

### Expense Detail Management

| Operation | Method | Route | Request | Result | Application Operation |
|---|---|---|---|---|---|
| Add Expense Detail | POST | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/details` | `AddExpenseDetailRequest` | `AddExpenseDetailResult` | `AddExpenseDetailCommand` |

---

### Investment Management

| Operation | Method | Route | Request | Result | Application Operation |
|---|---|---|---|---|---|
| Add Investment | POST | `/api/v1/financial-periods/{financialPeriodId}/investments` | `AddInvestmentRequest` | `AddInvestmentResult` | `AddInvestmentCommand` |
| Confirm Investment | POST | `/api/v1/financial-periods/{financialPeriodId}/investments/{investmentId}/confirmation` | `ConfirmInvestmentRequest` | `ConfirmInvestmentResult` | `ConfirmInvestmentCommand` |

---

## Request Models

### CreateFinancialPeriodRequest

| Field | Type |
|---|---|
| Year | `int` |
| Month | `int` |

### SynchronizeCurrentFinancialPeriodRequest

| Field | Type |
|---|---|
| Year | `int` |
| Month | `int` |

### AddIncomeRequest

| Field | Type |
|---|---|
| Name | `string` |
| PlannedAmount | `decimal` |

### EnterIncomeRequest

| Field | Type |
|---|---|
| ActualAmount | `decimal` |

### AddRecurringExpenseRequest

| Field | Type |
|---|---|
| Name | `string` |
| PlannedAmount | `decimal` |

### AddFixedTermExpenseRequest

| Field | Type |
|---|---|
| Name | `string` |
| PlannedAmount | `decimal` |
| CurrentInstallment | `int` |
| TotalInstallments | `int` |

### AddDiscretionaryExpenseRequest

| Field | Type |
|---|---|
| Name | `string` |

### AddExpenseDetailRequest

| Field | Type | Required |
|---|---|---|
| Description | `string` | Yes |
| ActualAmount | `decimal` | Yes |
| TransactionDate | `DateOnly?` | No |
| PlannedAmount | `decimal?` | No |

### EnterExpenseRequest

| Field | Type |
|---|---|
| ActualAmount | `decimal` |

### AddInvestmentRequest

| Field | Type |
|---|---|
| Name | `string` |
| PlannedAmount | `decimal` |

### ConfirmInvestmentRequest

| Field | Type |
|---|---|
| ActualAmount | `decimal` |

---

## HTTP Behavior

### Successful Operations

The current API uses the following success responses:

| Operation Type | HTTP Status |
|---|---|
| Resource creation | `201 Created` |
| State transition or confirmation | `200 OK` |
| Query | `200 OK` |

Creation operations return a `Location` referencing the related financial period.

---

## Request Validation

Request validation is performed at the API boundary for the endpoints that accept validated request models.
When validation fails, the endpoint returns:

`400 Validation Problem`

Validation is currently present for:
- Add Income.
- Enter Income.
- Add Recurring Expense.
- Add Fixed-Term Expense.
- Add Discretionary Expense.
- Add Expense Detail.
- Enter Expense.
- Add Investment.
- Confirm Investment.
- CreateFinancialPeriodRequest.
- SynchronizeCurrentFinancialPeriodRequest.
- GetFinancialPeriodByPeriod query parameters (`year`, `month`).
  
---

## Exception Handling

The API uses centralized exception handlers based on ASP.NET Core `IExceptionHandler`.
Handled domain and application exceptions are transformed into standardized Problem Details responses.

### Financial Period Errors

| Condition | HTTP Status |
|---|---|
| Financial period not found | `404 Not Found` |
| Financial period already exists | `409 Conflict` |
| Financial period cannot be closed | `409 Conflict` |
| Financial period is closed | `409 Conflict` |
| Current financial period not available | `409 Conflict` |
| Financial period by year and month not found | `404 Not Found` |

### Income Errors

| Condition | HTTP Status |
|---|---|
| Income not found in financial period | `404 Not Found` |
| Income already entered | `409 Conflict` |

### Expense Errors

| Condition | HTTP Status |
|---|---|
| Expense not found in financial period | `404 Not Found` |
| Expense already entered | `409 Conflict` |
| Expense has details | `409 Conflict` |
| Expense has no details | `409 Conflict` |
| Expense detail date outside financial period | `400 Bad Request` |

### Investment Errors

| Condition | HTTP Status |
|---|---|
| Investment not found in financial period | `404 Not Found` |
| Investment already confirmed | `409 Conflict` |

---

## Current API Summary

The reviewed API currently exposes **16 Financial Period endpoints**:
| Area | Endpoint Count |
|---|---:|
| Financial Period Lifecycle | 6 |
| Income Management | 2 |
| Expense Management | 5 |
| Expense Detail Management | 1 |
| Investment Management | 2 |
| **Total** | **16** |

The implementation currently contains:
- 2 query endpoints.
- 14 command-oriented endpoints.
- API versioning through `v1`.
- Request validation for supported input models.
- Centralized Problem Details exception handling.
- Explicit application Commands and Queries behind the API endpoints.

---

## Scope Boundary

This inventory intentionally does not classify the API against the Financial Period User Flow requirements.
The following questions are deferred to the next tasks:
- Which User Flow capabilities are already supported by these endpoints?
- Which capabilities are only partially supported?
- Which required capabilities have no corresponding API operation?
- Which existing contracts require modification to support the designed UX?

Those questions are addressed by:

1. **Map User Flows to API Operations**
2. **Identify Backend Gaps**

---

## Related Documentation

- Financial Period User Flow
- Financial Period Dashboard
- Screen Map
- Interface States