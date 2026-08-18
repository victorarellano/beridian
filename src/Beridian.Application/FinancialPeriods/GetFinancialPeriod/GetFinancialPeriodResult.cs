namespace Beridian.Application.FinancialPeriods.GetFinancialPeriod;

public sealed record GetFinancialPeriodResult(
    Guid FinancialPeriodId,
    int Year,
    int Month,
    string Status,
    MoneyResult OpeningBalance,
    MoneyResult PlannedBalance,
    MoneyResult ActualBalance,
    IReadOnlyCollection<ExpenseResult> Expenses,
    IReadOnlyCollection<IncomeResult> Incomes,
    IReadOnlyCollection<InvestmentResult> Investments);

public sealed record MoneyResult(
    decimal Amount, 
    string Currency);

public sealed record ExpenseResult(
    Guid ExpenseId,
    string Type,
    string Name,
    string Status,
    MoneyResult PlannedAmount,
    MoneyResult ActualAmount,
    int? CurrentInstallment,
    int? TotalInstallments,
    IReadOnlyCollection<ExpenseDetailResult> Details);

public sealed record ExpenseDetailResult(
    Guid ExpenseDetailId,
    string Description,
    DateOnly? TransactionDate,
    MoneyResult? PlannedAmount,
    MoneyResult ActualAmount);

public sealed record IncomeResult(
    Guid IncomeId,
    string Name,
    string Status,
    MoneyResult PlannedAmount,
    MoneyResult ActualAmount);

public sealed record InvestmentResult(
    Guid InvestmentId,
    string Name,
    string Status,
    MoneyResult PlannedAmount,
    MoneyResult ActualAmount);