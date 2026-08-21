namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseAlreadyEnteredException
    : InvalidOperationException
{
    public Guid ExpenseId { get; }

    public ExpenseAlreadyEnteredException(Guid expenseId)
        : base($"Expense '{expenseId}' has already been entered.")
    {
        ExpenseId = expenseId;
    }
}