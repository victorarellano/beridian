using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods.Events;
using Beridian.Domain.Incomes;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    public void Close()
    {
        EnsureIsOpen();

        if (_expenses.Any(expense => expense.Status != ExpenseStatus.Entered))
        {
            throw new InvalidOperationException("The financial period cannot be closed while expenses remain unentered.");
        }

        if (_incomes.Any(income => income.Status != IncomeStatus.Entered))
        {
            throw new InvalidOperationException("The financial period cannot be closed while incomes remain unentered.");
        }

        Status = FinancialPeriodStatus.Closed;

        RaiseDomainEvent(new FinancialPeriodClosed(Id, Period));
    }
}
