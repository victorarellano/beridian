namespace Beridian.Application.Expenses.AddDiscretionaryExpense;

public sealed record AddDiscretionaryExpenseCommand(Guid FinancialPeriodId, string Name);