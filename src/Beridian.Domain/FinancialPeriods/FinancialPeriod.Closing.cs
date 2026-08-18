using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods.Events;
using Beridian.Domain.FinancialPeriods.Exceptions;
using Beridian.Domain.Incomes;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    public void Close()
    {
        EnsureIsOpen();

        if (_expenses.Any(expense => expense.Status != ExpenseStatus.Entered))
        {
            throw new FinancialPeriodCannotBeClosedException(Id, FinancialPeriodClosingFailureReason.UnenteredExpenses);            
        }

        if (_incomes.Any(income => income.Status != IncomeStatus.Entered))
        {
            throw new FinancialPeriodCannotBeClosedException(Id, FinancialPeriodClosingFailureReason.UnenteredIncomes);            
        }

        Status = FinancialPeriodStatus.Closed;

        RaiseDomainEvent(new FinancialPeriodClosed(Id, Period));
    }
}