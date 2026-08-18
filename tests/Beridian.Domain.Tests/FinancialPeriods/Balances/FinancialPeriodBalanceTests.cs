using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.Tests.FinancialPeriods.Balances;

public sealed class FinancialPeriodBalanceTests
{
    [Fact]
    public void PlannedBalance_ShouldBeCalculatedFromContainedEntities()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.AddIncome(Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp)));

        financialPeriod.AddExpense(RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp)));

        financialPeriod.AddInvestment(Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp)));

        Assert.Equal(1_200_000m, financialPeriod.PlannedBalance.Amount);
    }

    [Fact]
    public void ActualBalance_ShouldBeCalculatedFromContainedEntities()
    {
        var financialPeriod = CreateFinancialPeriod();

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));
        income.Enter(Money.Create(1_520_000m, Currency.Clp));

        var expense = RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));
        expense.Enter(Money.Create(48_000m, Currency.Clp));

        var investment = Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp));
        investment.Confirm(Money.Create(220_000m, Currency.Clp));

        financialPeriod.AddIncome(income);
        financialPeriod.AddExpense(expense);
        financialPeriod.AddInvestment(investment);

        Assert.Equal(1_252_000m, financialPeriod.ActualBalance.Amount);
    }

    [Fact]
    public void Balances_WithoutContainedEntities_ShouldBeZero()
    {
        var financialPeriod = CreateFinancialPeriod();

        Assert.Equal(0m, financialPeriod.PlannedBalance.Amount);
        Assert.Equal(0m, financialPeriod.ActualBalance.Amount);
        Assert.Equal(Currency.Clp, financialPeriod.PlannedBalance.Currency);
        Assert.Equal(Currency.Clp, financialPeriod.ActualBalance.Currency);
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));
    }
}