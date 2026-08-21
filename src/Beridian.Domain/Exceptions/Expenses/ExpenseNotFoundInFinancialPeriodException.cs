namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseNotFoundInFinancialPeriodException : InvalidOperationException
{
    public Guid FinancialPeriodId { get; }

    public Guid ExpenseId { get; }

    public ExpenseNotFoundInFinancialPeriodException(
        Guid financialPeriodId,
        Guid expenseId)
        : base($"Expense '{expenseId}' was not found in financial period '{financialPeriodId}'.")
    {
        FinancialPeriodId = financialPeriodId;
        ExpenseId = expenseId;
    }
}