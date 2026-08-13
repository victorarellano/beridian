using Beridian.Domain.Expenses;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    public void AddExpense(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);

        EnsureIsOpen();

        if (_expenses.Any(current => current.Id == expense.Id))
        {
            throw new InvalidOperationException("The expense already belongs to this financial period.");
        }

        _expenses.Add(expense);
    }

    public void AddIncome(Income income)
    {
        ArgumentNullException.ThrowIfNull(income);

        EnsureIsOpen();

        if (_incomes.Any(current => current.Id == income.Id))
        {
            throw new InvalidOperationException("The income already belongs to this financial period.");
        }

        _incomes.Add(income);
    }

    public void AddInvestment(Investment investment)
    {
        ArgumentNullException.ThrowIfNull(investment);

        EnsureIsOpen();

        if (_investments.Any(current => current.Id == investment.Id))
        {
            throw new InvalidOperationException("The investment already belongs to this financial period.");
        }

        _investments.Add(investment);
    }
}