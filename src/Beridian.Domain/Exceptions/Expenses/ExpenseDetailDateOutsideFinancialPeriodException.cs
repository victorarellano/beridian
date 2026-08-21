namespace Beridian.Domain.Expenses.Exceptions;

public sealed class ExpenseDetailDateOutsideFinancialPeriodException : InvalidOperationException
{
    public Guid FinancialPeriodId { get; }

    public Guid ExpenseId { get; }

    public DateOnly TransactionDate { get; }

    public int Year { get; }

    public int Month { get; }

    public ExpenseDetailDateOutsideFinancialPeriodException(
        Guid financialPeriodId,
        Guid expenseId,
        DateOnly transactionDate,
        int year,
        int month)
        : base($"Expense detail date '{transactionDate:yyyy-MM-dd}' does not belong to financial period {year:D4}-{month:D2}.")
    {
        FinancialPeriodId = financialPeriodId;
        ExpenseId = expenseId;
        TransactionDate = transactionDate;
        Year = year;
        Month = month;
    }
}