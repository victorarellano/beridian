using Beridian.Domain.Common;

namespace Beridian.Domain.Expenses;

public abstract class Expense
{
    private readonly List<ExpenseDetail> _details = [];

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Money PlannedAmount { get; protected set; }

    public Money ActualAmount { get; private set; }

    public ExpenseStatus Status { get; private set; }

    public IReadOnlyCollection<ExpenseDetail> Details =>
        _details.AsReadOnly();

    protected Expense()
    {
        Name = null!;
        PlannedAmount = null!;
        ActualAmount = null!;
    }

    protected Expense(Guid id, string name, Money plannedAmount)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Expense identity cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Expense name cannot be empty.", nameof(name));
        }

        ArgumentNullException.ThrowIfNull(plannedAmount);

        if (plannedAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(plannedAmount), "Planned expense amount cannot be negative.");
        }

        Id = id;
        Name = name.Trim();
        PlannedAmount = plannedAmount;
        ActualAmount = Money.Zero(plannedAmount.Currency);
        Status = ExpenseStatus.Created;
    }

    internal void AddDetail(ExpenseDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);

        EnsureCreated();

        if (detail.ActualAmount.Currency != PlannedAmount.Currency)
        {
            throw new InvalidOperationException("Expense detail currency must match the expense currency.");
        }

        _details.Add(detail);
        ActualAmount = CalculateActualAmount();
    }

    internal void Enter(Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        EnsureCreated();

        if (_details.Count > 0)
        {
            throw new InvalidOperationException("An expense with details must be entered using its detail amounts.");
        }

        if (actualAmount.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(actualAmount), "Actual expense amount cannot be negative.");
        }

        if (actualAmount.Currency != PlannedAmount.Currency)
        {
            throw new InvalidOperationException("Actual amount currency must match the expense currency.");
        }

        ActualAmount = actualAmount;
        Status = ExpenseStatus.Entered;
    }

    internal void Enter()
    {
        EnsureCreated();

        if (_details.Count == 0)
        {
            throw new InvalidOperationException("An expense without details requires an actual amount.");
        }

        ActualAmount = CalculateActualAmount();
        Status = ExpenseStatus.Entered;
    }

    internal abstract Expense? CopyToNextPeriod();

    private void EnsureCreated()
    {
        if (Status == ExpenseStatus.Entered)
        {
            throw new InvalidOperationException("The expense has already been entered.");
        }
    }

    private Money CalculateActualAmount()
    {
        var total = Money.Zero(PlannedAmount.Currency);

        foreach (var detail in _details)
        {
            total = total.Add(detail.ActualAmount);
        }

        return total;
    }
}