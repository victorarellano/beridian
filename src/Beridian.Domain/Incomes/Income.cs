using Beridian.Domain.Common;

namespace Beridian.Domain.Incomes;

public sealed class Income
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Money PlannedAmount { get; private set; }

    public Money ActualAmount { get; private set; }

    public IncomeStatus Status { get; private set; }

    private Income()
    {
        Name = null!;
        PlannedAmount = null!;
        ActualAmount = null!;
    }

    private Income(Guid id, string name, Money plannedAmount)
    {
        Id = id;
        Name = name;
        PlannedAmount = plannedAmount;
        ActualAmount = Money.Zero(plannedAmount.Currency);
        Status = IncomeStatus.Created;
    }

    public static Income Create(string name, Money plannedAmount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Income name cannot be empty.",
                nameof(name));
        }

        ArgumentNullException.ThrowIfNull(plannedAmount);

        if (plannedAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedAmount),
                "Planned income amount cannot be negative.");
        }

        return new Income(Guid.NewGuid(), name.Trim(), plannedAmount);
    }

    internal void Enter(Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        if (Status == IncomeStatus.Entered)
        {
            throw new InvalidOperationException(
                "The income has already been entered.");
        }

        if (actualAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(actualAmount),
                "Actual income amount cannot be negative.");
        }

        if (actualAmount.Currency != PlannedAmount.Currency)
        {
            throw new InvalidOperationException(
                "Actual amount currency must match the income currency.");
        }

        ActualAmount = actualAmount;
        Status = IncomeStatus.Entered;
    }

    internal Income CopyToNextPeriod()
    {
        return Create(Name, ActualAmount);
    }
}