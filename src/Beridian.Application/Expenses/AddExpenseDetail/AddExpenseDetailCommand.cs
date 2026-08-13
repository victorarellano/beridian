namespace Beridian.Application.Expenses.AddExpenseDetail;

public sealed record AddExpenseDetailCommand(
    Guid FinancialPeriodId,
    Guid ExpenseId,
    string Description,
    decimal ActualAmount,
    DateOnly? TransactionDate = null,
    decimal? PlannedAmount = null);