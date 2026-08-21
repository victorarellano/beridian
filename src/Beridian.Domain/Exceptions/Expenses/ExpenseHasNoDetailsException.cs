namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseHasNoDetailsException : Exception
{
    public Guid ExpenseId { get; }

    public ExpenseHasNoDetailsException(Guid expenseId) : base($"Expense '{expenseId}' has no details.")
    {
        ExpenseId = expenseId;
    }
}