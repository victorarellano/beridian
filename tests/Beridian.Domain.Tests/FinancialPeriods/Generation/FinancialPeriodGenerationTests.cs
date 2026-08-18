using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;
using Beridian.Domain.Services;

namespace Beridian.Domain.Tests.FinancialPeriods.Generation;

public sealed class FinancialPeriodGenerationTests
{
    [Fact]
    public void GenerateNext_ShouldCreateFollowingMonthlyPeriod()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(2026, nextPeriod.Period.Year);
        Assert.Equal(9, nextPeriod.Period.Month);
    }

    [Fact]
    public void GenerateNext_WhenCurrentPeriodIsDecember_ShouldCreateJanuaryOfFollowingYear()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 12));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(2027, nextPeriod.Period.Year);
        Assert.Equal(1, nextPeriod.Period.Month);
    }

    [Fact]
    public void GenerateNext_ShouldCreateDifferentIdentity()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.NotEqual(currentPeriod.Id, nextPeriod.Id);
        Assert.NotEqual(Guid.Empty, nextPeriod.Id);
    }

    [Fact]
    public void GenerateNext_ShouldCreateOpenFinancialPeriod()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(FinancialPeriodStatus.Open, nextPeriod.Status);
    }

    [Fact]
    public void GenerateNext_ShouldTransferCurrentActualBalance()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        var expense = RecurringExpense.Create("Electricity", Money.Create(50_000m, Currency.Clp));

        currentPeriod.AddIncome(income);
        currentPeriod.AddExpense(expense);

        currentPeriod.EnterIncome(income.Id, Money.Create(1_520_000m, Currency.Clp));

        currentPeriod.EnterExpense(expense.Id, Money.Create(48_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(1_472_000m, nextPeriod.OpeningBalance.Amount.Amount);
    }

    [Fact]
    public void GenerateNext_WhenCurrentPeriodIsOpen_ShouldBeAllowed()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(FinancialPeriodStatus.Open, currentPeriod.Status);

        Assert.Equal(FinancialPeriodStatus.Open, nextPeriod.Status);
    }

    [Fact]
    public void GenerateNext_ShouldNotCopyExpensesOrInvestmentsYet()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Empty(nextPeriod.Expenses);
        Assert.Empty(nextPeriod.Investments);
    }

    [Fact]
    public void GenerateNext_ShouldCopyIncomeUsingPreviousActualAmountAsPlannedAmount()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        currentPeriod.AddIncome(income);

        currentPeriod.EnterIncome(income.Id, Money.Create(1_520_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        var copiedIncome = Assert.Single(nextPeriod.Incomes);

        Assert.Equal("Salary", copiedIncome.Name);
        Assert.Equal(1_520_000m, copiedIncome.PlannedAmount.Amount);
        Assert.Equal(0m, copiedIncome.ActualAmount.Amount);
        Assert.Equal(IncomeStatus.Created, copiedIncome.Status);
    }

    [Fact]
    public void GenerateNext_ShouldCreateNewIncomeIdentity()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var income = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        currentPeriod.AddIncome(income);

        currentPeriod.EnterIncome(income.Id, Money.Create(1_520_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        var copiedIncome = Assert.Single(nextPeriod.Incomes);

        Assert.NotEqual(income.Id, copiedIncome.Id);
        Assert.NotEqual(Guid.Empty, copiedIncome.Id);
    }

    [Fact]
    public void GenerateNext_ShouldCopyAllIncomes()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var salary = Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));

        var freelance = Income.Create("Freelance", Money.Create(200_000m, Currency.Clp));

        currentPeriod.AddIncome(salary);
        currentPeriod.AddIncome(freelance);

        currentPeriod.EnterIncome(salary.Id, Money.Create(1_520_000m, Currency.Clp));

        currentPeriod.EnterIncome(freelance.Id, Money.Create(180_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Equal(2, nextPeriod.Incomes.Count);
    }

    [Fact]
    public void Generate_ShouldNotCopyInvestments()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var investment = Investment.Create("Savings", Money.Create(250_000m, Currency.Clp));

        currentPeriod.AddInvestment(investment);

        var generator = new FinancialPeriodGenerator();

        var nextPeriod = generator.Generate(currentPeriod);

        Assert.Empty(nextPeriod.Investments);
    }

    [Fact]
    public void Generate_ShouldCreatePeriodUsingTransferredActualBalance()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var generator = new FinancialPeriodGenerator();

        var nextPeriod = generator.Generate(currentPeriod);

        Assert.Equal(currentPeriod.ActualBalance, nextPeriod.OpeningBalance.Amount);

        Assert.Equal(currentPeriod.Period.Next(), nextPeriod.Period);

        Assert.Equal(FinancialPeriodStatus.Open, nextPeriod.Status);
    }
}