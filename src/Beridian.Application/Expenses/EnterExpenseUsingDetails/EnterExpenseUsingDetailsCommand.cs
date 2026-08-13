namespace Beridian.Application.Expenses.EnterExpenseUsingDetails;

public sealed record EnterExpenseUsingDetailsCommand(Guid FinancialPeriodId, Guid ExpenseId);