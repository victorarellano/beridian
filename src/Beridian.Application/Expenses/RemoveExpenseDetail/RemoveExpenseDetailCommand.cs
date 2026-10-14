namespace Beridian.Application.Expenses.RemoveExpenseDetail;

public sealed record RemoveExpenseDetailCommand(Guid FinancialPeriodId, Guid ExpenseId, Guid ExpenseDetailId);