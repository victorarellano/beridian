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
        Validate(description, actualAmount, plannedAmount);

        return new ExpenseDetail(Guid.NewGuid(), description.Trim(), transactionDate, plannedAmount, actualAmount);
    }

    public void Update(string description, Money actualAmount, DateOnly? transactionDate, Money? plannedAmount)
    {
        Validate(description, actualAmount, plannedAmount);

        Description = description.Trim();
        ActualAmount = actualAmount;
        TransactionDate = transactionDate;
        PlannedAmount = plannedAmount;
    }

    private static void Validate(string description, Money actualAmount, Money? plannedAmount)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Expense detail description cannot be empty.", nameof(description));
        }

        ArgumentNullException.ThrowIfNull(actualAmount);

        if (actualAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(actualAmount), "Expense detail actual amount cannot be negative.");
        }

        if (plannedAmount is null)
        {
            return;
        }

        if (plannedAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(plannedAmount), "Expense detail planned amount cannot be negative.");
        }

        if (plannedAmount.Currency != actualAmount.Currency)
        {
            throw new InvalidOperationException("Planned and actual amounts must use the same currency.");
        }
    }    
}