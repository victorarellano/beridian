using Beridian.Domain.Common;

namespace Beridian.Domain.Expenses;

public sealed class RecurringExpense : Expense
{
    private RecurringExpense()
    {
    }

    private RecurringExpense(Guid id, string name, Money plannedAmount)
        : base(id, name, plannedAmount)
    {
    }

    public static RecurringExpense Create(string name, Money plannedAmount)
    {
        return new RecurringExpense(Guid.NewGuid(), name, plannedAmount);
    }

    internal override Expense CopyToNextPeriod()
    {
        return Create(Name, ActualAmount);
    }
}