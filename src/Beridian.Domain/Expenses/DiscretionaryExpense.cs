using Beridian.Domain.Common;

namespace Beridian.Domain.Expenses;

public sealed class DiscretionaryExpense : Expense
{
    private DiscretionaryExpense()
    {
    }

    private DiscretionaryExpense(Guid id, string name, Money plannedAmount)
        : base(id, name, plannedAmount)
    {
    }

    public static DiscretionaryExpense Create(string name, Currency currency)
    {
        return new DiscretionaryExpense(Guid.NewGuid(), name, Money.Zero(currency));
    }

    internal override Expense CopyToNextPeriod()
    {
        return Create(Name, PlannedAmount.Currency);
    }
}