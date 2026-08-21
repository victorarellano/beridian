
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Expenses.Exceptions;
using Beridian.Domain.Incomes;
using Beridian.Domain.Incomes.Exceptions;
using Beridian.Domain.Investments;
using Beridian.Domain.Investments.Exceptions;

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
            throw new ExpenseDetailDateOutsideFinancialPeriodException(
                Id,
                expenseId,
                detail.TransactionDate.Value,
                Period.Year,
                Period.Month);
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
            throw new ExpenseNotFoundInFinancialPeriodException(Id, expenseId);
    }
    private Income FindIncome(Guid incomeId)
    {
        return _incomes.SingleOrDefault(income => income.Id == incomeId) ??
            throw new IncomeNotFoundInFinancialPeriodException(Id, incomeId);
    }
    private Investment FindInvestment(Guid investmentId)
    {
        return _investments.SingleOrDefault(investment => investment.Id == investmentId) ??
        throw new InvestmentNotFoundInFinancialPeriodException(Id, investmentId);
    }
}
