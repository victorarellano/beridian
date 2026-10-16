# Beridian — Shared Frontend Models, DTOs and Mappers

**Status:** Approved design baseline from UF-001; future flows may extend it.

This document contains reusable types and mapping contracts. For the entry-flow coordination see [UF-001](user-flows/uf-001-application-entry.md).

## API DTOs — approved contracts

```typescript
export interface SynchronizeFinancialPeriodRequestDto {
  year: number;
  month: number;
}

export interface SynchronizeCurrentFinancialPeriodResponseDto {
  financialPeriodId: string;
  year: number;
  month: number;
  wasCreated: boolean;
}

export enum FinancialPeriodStatusDto {
  Open = 'Open',
  Closed = 'Closed'
}

export enum CurrencyDto {
  Clp = 'Clp',
  Usd = 'Usd'
}

export interface MoneyDto {
  amount: number;
  currency: CurrencyDto;
}

export enum ExpenseTypeDto {
  Recurring = 'Recurring',
  FixedTerm = 'FixedTerm',
  Discretionary = 'Discretionary'
}

export enum ExpenseStatusDto {
  Created = 'Created',
  Entered = 'Entered'
}

export interface ExpenseDetailDto {
  expenseDetailId: string;
  description: string;
  transactionDate: string | null;
  plannedAmount: MoneyDto | null;
  actualAmount: MoneyDto;
}

export interface ExpenseDto {
  expenseId: string;
  type: ExpenseTypeDto;
  name: string;
  status: ExpenseStatusDto;
  plannedAmount: MoneyDto;
  actualAmount: MoneyDto;
  currentInstallment: number | null;
  totalInstallments: number | null;
  details: ExpenseDetailDto[];
}

export enum IncomeStatusDto {
  Created = 'Created',
  Entered = 'Entered'
}

export interface IncomeDto {
  incomeId: string;
  name: string;
  status: IncomeStatusDto;
  plannedAmount: MoneyDto;
  actualAmount: MoneyDto;
}

export enum InvestmentStatusDto {
  Created = 'Created',
  Confirmed = 'Confirmed'
}

export interface InvestmentDto {
  investmentId: string;
  name: string;
  status: InvestmentStatusDto;
  plannedAmount: MoneyDto;
  actualAmount: MoneyDto;
}

export interface FinancialPeriodResponseDto {
  financialPeriodId: string;
  year: number;
  month: number;
  status: FinancialPeriodStatusDto;
  openingBalance: MoneyDto;
  plannedBalance: MoneyDto;
  actualBalance: MoneyDto;
  expenses: ExpenseDto[];
  incomes: IncomeDto[];
  investments: InvestmentDto[];
}
```

**Contract notes:** C# `Guid` → TypeScript `string`; `int`/`decimal` → `number`; `bool` → `boolean`; nullable C# values → `T | null`; collections → arrays. The DTO field names assume the API's camelCase JSON convention and must be checked against HTTP integration responses during implementation. `HttpClient<T>` provides compile-time typing but does not itself validate JSON at runtime. `decimal` → JavaScript `number` is not exact for every decimal value; critical financial arithmetic stays in the backend.

The backend's domain enums use numeric backing values, but the existing result mapper calls `ToString()`: transport values are therefore strings such as `Open`, `Clp`, `Recurring`, `Entered`, and `Confirmed`.

## Frontend models — approved contracts

```typescript
export enum FinancialPeriodStatus {
  Open = 'Open',
  Closed = 'Closed'
}

export enum Currency {
  Clp = 'Clp',
  Usd = 'Usd'
}

export interface Money {
  amount: number;
  currency: Currency;
}

export enum ExpenseType {
  Recurring = 'Recurring',
  FixedTerm = 'FixedTerm',
  Discretionary = 'Discretionary'
}

export enum ExpenseStatus {
  Created = 'Created',
  Entered = 'Entered'
}

export interface ExpenseDetail {
  id: string;
  description: string;
  transactionDate: string | null;
  plannedAmount: Money | null;
  actualAmount: Money;
}

export interface Expense {
  id: string;
  type: ExpenseType;
  name: string;
  status: ExpenseStatus;
  plannedAmount: Money;
  actualAmount: Money;
  currentInstallment: number | null;
  totalInstallments: number | null;
  details: ExpenseDetail[];
}

export enum IncomeStatus {
  Created = 'Created',
  Entered = 'Entered'
}

export interface Income {
  id: string;
  name: string;
  status: IncomeStatus;
  plannedAmount: Money;
  actualAmount: Money;
}

export enum InvestmentStatus {
  Created = 'Created',
  Confirmed = 'Confirmed'
}

export interface Investment {
  id: string;
  name: string;
  status: InvestmentStatus;
  plannedAmount: Money;
  actualAmount: Money;
}

export interface FinancialPeriod {
  id: string;
  year: number;
  month: number;
  status: FinancialPeriodStatus;
  openingBalance: Money;
  plannedBalance: Money;
  actualBalance: Money;
  expenses: Expense[];
  incomes: Income[];
  investments: Investment[];
}
```

Frontend models use `id` rather than transport-specific names such as `expenseId`. They do not include UI-specific strings/colors or financial business logic. Distinct enums remain independent even when their current values coincide.

## Mapper contracts and composition

**Approved public mapping operations:**

```typescript
mapFinancialPeriod(dto: FinancialPeriodResponseDto): FinancialPeriod
mapMoney(dto: MoneyDto): Money
mapExpense(dto: ExpenseDto): Expense
mapExpenseDetail(dto: ExpenseDetailDto): ExpenseDetail
mapIncome(dto: IncomeDto): Income
mapInvestment(dto: InvestmentDto): Investment
```

**Enum conversion operations** follow the same explicit `Record` convention: `mapFinancialPeriodStatus`, `mapCurrency`, `mapExpenseType`, `mapExpenseStatus`, `mapIncomeStatus`, and `mapInvestmentStatus`.

Illustrative mapping code (imports omitted for readability):

```typescript
const currencyMap: Record<CurrencyDto, Currency> = {
  [CurrencyDto.Clp]: Currency.Clp,
  [CurrencyDto.Usd]: Currency.Usd
};

export function mapCurrency(dto: CurrencyDto): Currency {
  const value = currencyMap[dto];
  if (value === undefined) {
    throw new Error(`Unsupported currency: ${dto}`);
  }
  return value;
}

export function mapMoney(dto: MoneyDto): Money {
  return { amount: dto.amount, currency: mapCurrency(dto.currency) };
}

export function mapExpenseDetail(dto: ExpenseDetailDto): ExpenseDetail {
  return {
    id: dto.expenseDetailId,
    description: dto.description,
    transactionDate: dto.transactionDate,
    plannedAmount: dto.plannedAmount === null ? null : mapMoney(dto.plannedAmount),
    actualAmount: mapMoney(dto.actualAmount)
  };
}

export function mapExpense(dto: ExpenseDto): Expense {
  return {
    id: dto.expenseId,
    type: mapExpenseType(dto.type),
    name: dto.name,
    status: mapExpenseStatus(dto.status),
    plannedAmount: mapMoney(dto.plannedAmount),
    actualAmount: mapMoney(dto.actualAmount),
    currentInstallment: dto.currentInstallment,
    totalInstallments: dto.totalInstallments,
    details: dto.details.map(mapExpenseDetail)
  };
}

export function mapIncome(dto: IncomeDto): Income {
  return {
    id: dto.incomeId,
    name: dto.name,
    status: mapIncomeStatus(dto.status),
    plannedAmount: mapMoney(dto.plannedAmount),
    actualAmount: mapMoney(dto.actualAmount)
  };
}

export function mapInvestment(dto: InvestmentDto): Investment {
  return {
    id: dto.investmentId,
    name: dto.name,
    status: mapInvestmentStatus(dto.status),
    plannedAmount: mapMoney(dto.plannedAmount),
    actualAmount: mapMoney(dto.actualAmount)
  };
}

export function mapFinancialPeriod(dto: FinancialPeriodResponseDto): FinancialPeriod {
  return {
    id: dto.financialPeriodId,
    year: dto.year,
    month: dto.month,
    status: mapFinancialPeriodStatus(dto.status),
    openingBalance: mapMoney(dto.openingBalance),
    plannedBalance: mapMoney(dto.plannedBalance),
    actualBalance: mapMoney(dto.actualBalance),
    expenses: dto.expenses.map(mapExpense),
    incomes: dto.incomes.map(mapIncome),
    investments: dto.investments.map(mapInvestment)
  };
}
```

The remaining enum lookup tables follow the same pattern as `currencyMap`; their values are fixed by the approved DTO/model enums above. The `Record` type ensures exhaustive coverage of the declared source enum at compile time. A runtime `undefined` check rejects unexpected transport values, but does not replace full response-schema validation.
