namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseDetailNotFoundException : InvalidOperationException
{
    public Guid ExpenseId { get; }
    public Guid ExpenseDetailId { get; }
    public ExpenseDetailNotFoundException(Guid expenseId, Guid expenseDetailId)
        : base($"Expense detail '{expenseDetailId}' was not found " + $"in expense '{expenseId}'.")
    {
        ExpenseId = expenseId;
        ExpenseDetailId = expenseDetailId;
    }
}