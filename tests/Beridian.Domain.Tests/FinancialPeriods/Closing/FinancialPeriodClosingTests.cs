using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.Tests.FinancialPeriods.Closing;

public sealed class FinancialPeriodClosingTests
{
    [Fact]
    public void Close_WhenAllExpensesAndIncomesAreEntered_ShouldClosePeriod()
    {
        var financialPeriod = CreateFinancialPeriod();

        var expense = RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));

        expense.Enter(Money.Create(48_000m, Currency.Clp));

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        income.Enter(Money.Create(1_520_000m, Currency.Clp));

        financialPeriod.AddExpense(expense);
        financialPeriod.AddIncome(income);

        financialPeriod.Close();

        Assert.Equal(FinancialPeriodStatus.Closed, financialPeriod.Status);
    }

    [Fact]
    public void Close_WithUnenteredExpense_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.AddExpense(RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp)));

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.Close());

        Assert.Equal(FinancialPeriodStatus.Open, financialPeriod.Status);
    }

    [Fact]
    public void Close_WithUnenteredIncome_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.AddIncome(Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp)));

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.Close());

        Assert.Equal(
            FinancialPeriodStatus.Open,
            financialPeriod.Status);
    }

    [Fact]
    public void Close_WithUnconfirmedInvestment_ShouldClosePeriod()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.AddInvestment(Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp)));

        financialPeriod.Close();

        Assert.Equal(
            FinancialPeriodStatus.Closed,
            financialPeriod.Status);
    }

    [Fact]
    public void Close_WhenAlreadyClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.Close());
    }

    [Fact]
    public void AddExpense_WhenPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        var expense = RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.AddExpense(expense));
    }

    [Fact]
    public void AddIncome_WhenPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        Assert.Throws<FinancialPeriodCannotBeClosedException>(() => financialPeriod.AddIncome(income));
    }

    [Fact]
    public void AddInvestment_WhenPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        var investment = Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp));

        Assert.Throws<FinancialPeriodCannotBeClosedException>(
            () => financialPeriod.AddInvestment(investment));
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));
    }
}