using Beridian.Domain.Common;

namespace Beridian.Domain.Investments;

public sealed class Investment
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Money PlannedAmount { get; private set; }

    public Money ActualAmount { get; private set; }

    public InvestmentStatus Status { get; private set; }

    private Investment()
    {
        Name = null!;
        PlannedAmount = null!;
        ActualAmount = null!;
    }

    private Investment(Guid id, string name, Money plannedAmount)
    {
        Id = id;
        Name = name;
        PlannedAmount = plannedAmount;
        ActualAmount = Money.Zero(plannedAmount.Currency);
        Status = InvestmentStatus.Created;
    }

    public static Investment Create(string name, Money plannedAmount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Investment name cannot be empty.",
                nameof(name));
        }

        ArgumentNullException.ThrowIfNull(plannedAmount);

        if (plannedAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedAmount),
                "Planned investment amount cannot be negative.");
        }

        return new Investment(Guid.NewGuid(), name.Trim(), plannedAmount);
    }

    internal void Confirm(Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        if (Status == InvestmentStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "The investment has already been confirmed.");
        }

        if (actualAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(actualAmount),
                "Actual investment amount cannot be negative.");
        }

        if (actualAmount.Currency != PlannedAmount.Currency)
        {
            throw new InvalidOperationException(
                "Actual amount currency must match the investment currency.");
        }

        ActualAmount = actualAmount;
        Status = InvestmentStatus.Confirmed;
    }
}