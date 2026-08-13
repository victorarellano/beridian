using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Domain.Tests.FinancialPeriods.Operations;

public sealed class FinancialPeriodExpenseDetailTests
{
    [Fact]
    public void AddExpenseDetail_WithContainedExpense_ShouldAddDetail()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));

        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Contains(detail, expense.Details);
        Assert.Single(expense.Details);
    }

    [Fact]
    public void AddExpenseDetail_WhenExpenseDoesNotBelongToPeriod_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var externalExpense = CreateExpense();

        var detail = CreateDetail("External Expense", new DateOnly(2026, 8, 5));

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddExpenseDetail(externalExpense.Id, detail));
    }

    [Fact]
    public void AddExpenseDetail_WithDateOutsidePeriod_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Outside Period Expense", new DateOnly(2026, 9, 1));

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddExpenseDetail(expense.Id, detail));

        Assert.Empty(expense.Details);
    }

    [Fact]
    public void AddExpenseDetail_WithoutTransactionDate_ShouldAddDetail()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = ExpenseDetail.Create(
            "Monthly medication", Money.Create(18_000m, Currency.Clp), plannedAmount: Money.Create(
                20_000m,
                Currency.Clp));

        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Contains(detail, expense.Details);
    }

    [Fact]
    public void AddExpenseDetail_WhenPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        financialPeriod.EnterExpense(expense.Id, Money.Create(48_000m, Currency.Clp));

        financialPeriod.Close();

        var detail = CreateDetail("Closed Period Expense", new DateOnly(2026, 8, 5));

        Assert.Throws<InvalidOperationException>(
            () => financialPeriod.AddExpenseDetail(expense.Id, detail));
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));
    }

    private static Expense CreateExpense()
    {
        return DiscretionaryExpense.Create("Personal Expenses", Currency.Clp);
    }

    private static ExpenseDetail CreateDetail(string detail, DateOnly transactionDate)
    {
        return ExpenseDetail.Create(detail, Money.Create(15_000m, Currency.Clp), transactionDate);
    }
}