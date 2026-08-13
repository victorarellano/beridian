
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    public void AddExpenseDetail(Guid expenseId, ExpenseDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);

        EnsureIsOpen();

        var expense = FindExpense(expenseId);

        if (detail.TransactionDate.HasValue && !Period.Contains(detail.TransactionDate.Value))
        {
            throw new InvalidOperationException("Expense detail date must belong to the financial period.");
        }

        expense.AddDetail(detail);
    }

    public void EnterExpense(Guid expenseId, Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        EnsureIsOpen();
        var expense = FindExpense(expenseId);
        expense.Enter(actualAmount);
    }

    public void EnterExpense(Guid expenseId)
    {
        EnsureIsOpen();
        var expense = FindExpense(expenseId);
        expense.Enter();
    }
    public void EnterIncome(Guid incomeId, Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        EnsureIsOpen();
        var income = FindIncome(incomeId);
        income.Enter(actualAmount);
    }
    public void ConfirmInvestment(Guid investmentId, Money actualAmount)
    {
        ArgumentNullException.ThrowIfNull(actualAmount);

        EnsureIsOpen();
        var investment = FindInvestment(investmentId);
        investment.Confirm(actualAmount);
    }


    private Expense FindExpense(Guid expenseId)
    {
        return _expenses.SingleOrDefault(expense => expense.Id == expenseId) ??
            throw new InvalidOperationException("The expense does not belong to this financial period.");
    }
    private Income FindIncome(Guid incomeId)
    {
        return _incomes.SingleOrDefault(income => income.Id == incomeId) ??
            throw new InvalidOperationException("The income does not belong to this financial period.");
    }
    private Investment FindInvestment(Guid investmentId)
    {
        return _investments.SingleOrDefault(investment => investment.Id == investmentId) ??
        throw new InvalidOperationException("The investment does not belong to this financial period.");
    }
}
