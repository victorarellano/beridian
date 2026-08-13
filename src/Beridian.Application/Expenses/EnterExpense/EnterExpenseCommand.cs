namespace Beridian.Application.Expenses.EnterExpense;

public sealed record EnterExpenseCommand(
    Guid FinancialPeriodId,
    Guid ExpenseId,
    decimal ActualAmount);