using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.Tests.FinancialPeriods.Composition;

public sealed class FinancialPeriodCompositionTests
{
    [Fact]
    public void AddExpense_WithValidExpense_ShouldIncludeExpense()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        Assert.Contains(expense, financialPeriod.Expenses);
        Assert.Single(financialPeriod.Expenses);
    }

    [Fact]
    public void AddIncome_WithValidIncome_ShouldIncludeIncome()
    {
        var financialPeriod = CreateFinancialPeriod();
        var income = CreateIncome();

        financialPeriod.AddIncome(income);

        Assert.Contains(income, financialPeriod.Incomes);
        Assert.Single(financialPeriod.Incomes);
    }

    [Fact]
    public void AddInvestment_WithValidInvestment_ShouldIncludeInvestment()
    {
        var financialPeriod = CreateFinancialPeriod();
        var investment = CreateInvestment();

        financialPeriod.AddInvestment(investment);

        Assert.Contains(investment, financialPeriod.Investments);
        Assert.Single(financialPeriod.Investments);
    }

    [Fact]
    public void AddExpense_WhenExpenseAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddExpense(expense));
    }

    [Fact]
    public void AddIncome_WhenIncomeAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var income = CreateIncome();

        financialPeriod.AddIncome(income);

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddIncome(income));
    }

    [Fact]
    public void AddInvestment_WhenInvestmentAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var investment = CreateInvestment();

        financialPeriod.AddInvestment(investment);

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddInvestment(investment));
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(Period.Create(2026, 8));
    }

    private static Expense CreateExpense()
    {
        return RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));
    }

    private static Income CreateIncome()
    {
        return Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));
    }

    private static Investment CreateInvestment()
    {
        return Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp));
    }
}