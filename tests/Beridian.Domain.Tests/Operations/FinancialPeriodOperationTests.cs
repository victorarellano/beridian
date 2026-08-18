using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.Tests.FinancialPeriods.Operations;

public sealed class FinancialPeriodOperationTests
{
    [Fact]
    public void EnterIncome_WithContainedIncome_ShouldEnterIncome()
    {
        var financialPeriod = CreateFinancialPeriod();
        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        financialPeriod.AddIncome(income);

        financialPeriod.EnterIncome(income.Id, Money.Create(1_520_000m, Currency.Clp));

        Assert.Equal(IncomeStatus.Entered, income.Status);
        Assert.Equal(1_520_000m, income.ActualAmount.Amount);
    }

    [Fact]
    public void EnterExpense_WithContainedExpense_ShouldEnterExpense()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));

        financialPeriod.AddExpense(expense);
        financialPeriod.EnterExpense(expense.Id, Money.Create(48_000m, Currency.Clp));

        Assert.Equal(ExpenseStatus.Entered, expense.Status);
        Assert.Equal(48_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void ConfirmInvestment_WithContainedInvestment_ShouldConfirmInvestment()
    {
        var financialPeriod = CreateFinancialPeriod();
        var investment = Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp));

        financialPeriod.AddInvestment(investment);
        financialPeriod.ConfirmInvestment(investment.Id, Money.Create(220_000m, Currency.Clp));

        Assert.Equal(InvestmentStatus.Confirmed, investment.Status);
        Assert.Equal(220_000m, investment.ActualAmount.Amount);
    }

    [Fact]
    public void EnterIncome_WhenPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        financialPeriod.AddIncome(income);
        financialPeriod.EnterIncome(income.Id, Money.Create(1_500_000m, Currency.Clp));

        financialPeriod.Close();

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.EnterIncome(income.Id, Money.Create(1_520_000m, Currency.Clp)));
    }

    [Fact]
    public void EnterIncome_WhenIncomeDoesNotBelongToPeriod_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        var externalIncome = Income.Create("External income", Money.Create(100_000m, Currency.Clp));

        Assert.Throws<InvalidOperationException>(() => financialPeriod.EnterIncome(externalIncome.Id, Money.Create(100_000m, Currency.Clp)));
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));
    }
}