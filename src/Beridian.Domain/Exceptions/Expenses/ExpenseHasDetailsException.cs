namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseHasDetailsException : Exception
{
    public Guid ExpenseId { get; }

    public ExpenseHasDetailsException(Guid expenseId)
        : base($"Expense '{expenseId}' has details and must be entered using its detail amounts.")
    {
        ExpenseId = expenseId;
    }
}