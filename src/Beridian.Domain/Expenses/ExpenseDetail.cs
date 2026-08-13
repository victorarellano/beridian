using Beridian.Domain.Common;

namespace Beridian.Domain.Expenses;

public sealed class ExpenseDetail
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public DateOnly? TransactionDate { get; private set; }
    public Money? PlannedAmount { get; private set; }
    public Money ActualAmount { get; private set; }

    private ExpenseDetail()
    {
        Description = null!;
        ActualAmount = null!;
    }

    private ExpenseDetail(Guid id, string description, DateOnly? transactionDate, Money? plannedAmount, Money actualAmount)
    {
        Id = id;
        Description = description;
        TransactionDate = transactionDate;
        PlannedAmount = plannedAmount;
        ActualAmount = actualAmount;
    }

    public static ExpenseDetail Create(string description, Money actualAmount, DateOnly? transactionDate = null, Money? plannedAmount = null)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Expense detail description cannot be empty.",
                nameof(description));
        }

        ArgumentNullException.ThrowIfNull(actualAmount);

        if (actualAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(actualAmount),
                "Expense detail actual amount cannot be negative.");
        }

        if (plannedAmount is not null)
        {
            if (plannedAmount.Amount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(plannedAmount),
                    "Expense detail planned amount cannot be negative.");
            }

            if (plannedAmount.Currency != actualAmount.Currency)
            {
                throw new InvalidOperationException(
                    "Planned and actual amounts must use the same currency.");
            }
        }

        return new ExpenseDetail(
            Guid.NewGuid(),
            description.Trim(),
            transactionDate,
            plannedAmount,
            actualAmount);
    }
}