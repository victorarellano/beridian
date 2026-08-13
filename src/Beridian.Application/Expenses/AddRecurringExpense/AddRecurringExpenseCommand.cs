namespace Beridian.Application.Expenses.AddRecurringExpense;

public sealed record AddRecurringExpenseCommand(Guid FinancialPeriodId, string Name, decimal PlannedAmount);