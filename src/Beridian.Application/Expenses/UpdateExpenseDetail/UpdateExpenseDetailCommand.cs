namespace Beridian.Application.Expenses.UpdateExpenseDetail;

public sealed record UpdateExpenseDetailCommand(Guid FinancialPeriodId, Guid ExpenseId, Guid ExpenseDetailId, string Description, decimal ActualAmount, DateOnly? TransactionDate = null, decimal? PlannedAmount = null);