using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Events;

namespace Beridian.Domain.Services;

public sealed class FinancialPeriodGenerator
{
    public FinancialPeriod Generate(FinancialPeriod currentPeriod)
    {
        ArgumentNullException.ThrowIfNull(currentPeriod);

        var transferredBalance = TransferredBalance.Create(currentPeriod.ActualBalance);

        var nextPeriod = FinancialPeriod.Create(currentPeriod.Period.Next(), transferredBalance);

        CopyIncomes(currentPeriod, nextPeriod);
        CopyExpenses(currentPeriod, nextPeriod);

        nextPeriod.RaiseDomainEvent(new FinancialPeriodGenerated(currentPeriod.Id, nextPeriod.Id, nextPeriod.Period));

        return nextPeriod;
    }

    private static void CopyIncomes(FinancialPeriod currentPeriod, FinancialPeriod nextPeriod)
    {
        foreach (var income in currentPeriod.Incomes)
        {
            nextPeriod.AddIncome(income.CopyToNextPeriod());
        }
    }

    private static void CopyExpenses(FinancialPeriod currentPeriod, FinancialPeriod nextPeriod)
    {
        foreach (var expense in currentPeriod.Expenses)
        {
            var copiedExpense = expense.CopyToNextPeriod();

            if (copiedExpense is not null)
            {
                nextPeriod.AddExpense(copiedExpense);
            }
        }
    }
}