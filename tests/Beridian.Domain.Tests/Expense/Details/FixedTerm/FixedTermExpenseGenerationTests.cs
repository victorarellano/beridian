using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Domain.Tests.Expenses.FixedTerm;

public sealed class FixedTermExpenseGenerationTests
{
    [Fact]
    public void GenerateNext_WhenInstallmentsRemain_ShouldCreateNextInstallment()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = FixedTermExpense.Create("Laptop", Money.Create(100_000m, Currency.Clp), currentInstallment: 2, totalInstallments: 6);

        currentPeriod.AddExpense(expense);

        currentPeriod.EnterExpense(expense.Id, Money.Create(95_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        var copiedExpense = Assert.IsType<FixedTermExpense>(Assert.Single(nextPeriod.Expenses));

        Assert.Equal(3, copiedExpense.CurrentInstallment);
        Assert.Equal(6, copiedExpense.TotalInstallments);
        Assert.Equal(95_000m, copiedExpense.PlannedAmount.Amount);
        Assert.Equal(0m, copiedExpense.ActualAmount.Amount);
        Assert.Equal(ExpenseStatus.Created, copiedExpense.Status);
        Assert.NotEqual(expense.Id, copiedExpense.Id);
    }

    [Fact]
    public void GenerateNext_WhenLastInstallmentWasPaid_ShouldNotCopyExpense()
    {
        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = FixedTermExpense.Create("Laptop", Money.Create(100_000m, Currency.Clp), currentInstallment: 6, totalInstallments: 6);

        currentPeriod.AddExpense(expense);

        currentPeriod.EnterExpense(expense.Id, Money.Create(95_000m, Currency.Clp));

        var nextPeriod = currentPeriod.GenerateNext();

        Assert.Empty(nextPeriod.Expenses);
    }
}