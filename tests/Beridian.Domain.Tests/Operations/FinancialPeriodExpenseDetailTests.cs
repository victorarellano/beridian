using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Expenses.Exceptions;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

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

        Assert.Throws<ExpenseNotFoundInFinancialPeriodException>(
            () => financialPeriod.AddExpenseDetail(externalExpense.Id, detail));
    }

    [Fact]
    public void AddExpenseDetail_WithDateOutsidePeriod_ShouldThrowInvalidOperationException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Outside Period Expense", new DateOnly(2026, 9, 1));

        Assert.Throws<ExpenseDetailDateOutsideFinancialPeriodException>(
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

        Assert.Throws<FinancialPeriodClosedException>(() => financialPeriod.AddExpenseDetail(expense.Id, detail));
    }

    [Fact]
    public void UpdateExpenseDetail_WithValidValues_ShouldUpdateDetail()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        var newDate = new DateOnly(2026, 8, 10);

        financialPeriod.UpdateExpenseDetail(expense.Id, detail.Id, "Dinner", Money.Create(25_000m, Currency.Clp), newDate, Money.Create(30_000m, Currency.Clp));

        Assert.Equal("Dinner", detail.Description);
        Assert.Equal(25_000m, detail.ActualAmount.Amount);
        Assert.Equal(30_000m, detail.PlannedAmount?.Amount);
        Assert.Equal(newDate, detail.TransactionDate);
        Assert.Equal(detail.Id, Assert.Single(expense.Details).Id);
    }

    [Fact]
    public void UpdateExpenseDetail_WhenActualAmountChanges_ShouldRecalculateExpenseActualAmount()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var firstDetail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        var secondDetail = CreateDetail("Dinner", new DateOnly(2026, 8, 6));

        financialPeriod.AddExpenseDetail(expense.Id, firstDetail);
        financialPeriod.AddExpenseDetail(expense.Id, secondDetail);

        Assert.Equal(30_000m, expense.ActualAmount.Amount);

        financialPeriod.UpdateExpenseDetail(expense.Id, firstDetail.Id, "Lunch updated", Money.Create(20_000m, Currency.Clp), new DateOnly(2026, 8, 5), null);

        Assert.Equal(35_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void UpdateExpenseDetail_WhenDetailDoesNotExist_ShouldThrowExpenseDetailNotFoundException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Throws<ExpenseDetailNotFoundException>(() =>
            financialPeriod.UpdateExpenseDetail(expense.Id, Guid.NewGuid(), "Dinner", Money.Create(20_000m, Currency.Clp), new DateOnly(2026, 8, 6), null));

        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void UpdateExpenseDetail_WithDateOutsidePeriod_ShouldThrowExpenseDetailDateOutsideFinancialPeriodException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Throws<ExpenseDetailDateOutsideFinancialPeriodException>(() =>
            financialPeriod.UpdateExpenseDetail(expense.Id, detail.Id, "Dinner", Money.Create(20_000m, Currency.Clp), new DateOnly(2026, 9, 1), null));

        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(new DateOnly(2026, 8, 5), detail.TransactionDate);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void UpdateExpenseDetail_WhenExpenseIsEntered_ShouldThrowExpenseAlreadyEnteredException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        financialPeriod.EnterExpense(expense.Id);

        Assert.Throws<ExpenseAlreadyEnteredException>(() =>
            financialPeriod.UpdateExpenseDetail(expense.Id, detail.Id, "Dinner", Money.Create(20_000m, Currency.Clp), new DateOnly(2026, 8, 6), null));

        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void RemoveExpenseDetail_WithExistingDetail_ShouldRemoveDetailAndRecalculateActualAmount()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var firstDetail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        var secondDetail = CreateDetail("Dinner", new DateOnly(2026, 8, 6));

        financialPeriod.AddExpenseDetail(expense.Id, firstDetail);
        financialPeriod.AddExpenseDetail(expense.Id, secondDetail);

        Assert.Equal(30_000m, expense.ActualAmount.Amount);

        financialPeriod.RemoveExpenseDetail(expense.Id, firstDetail.Id);

        Assert.Single(expense.Details);
        Assert.DoesNotContain(firstDetail, expense.Details);
        Assert.Contains(secondDetail, expense.Details);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void RemoveExpenseDetail_WhenDetailDoesNotExist_ShouldThrowExpenseDetailNotFoundException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Throws<ExpenseDetailNotFoundException>(() =>
            financialPeriod.RemoveExpenseDetail(expense.Id, Guid.NewGuid()));

        Assert.Single(expense.Details);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void RemoveExpenseDetail_WhenExpenseIsEntered_ShouldThrowExpenseAlreadyEnteredException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        financialPeriod.EnterExpense(expense.Id);

        Assert.Throws<ExpenseAlreadyEnteredException>(() =>
            financialPeriod.RemoveExpenseDetail(expense.Id, detail.Id));

        Assert.Single(expense.Details);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void RemoveExpenseDetail_WhenRemovingLastDetail_ShouldResetActualAmountToZero()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        financialPeriod.RemoveExpenseDetail(expense.Id, detail.Id);

        Assert.Empty(expense.Details);
        Assert.Equal(0m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void UpdateExpenseDetail_WithDifferentCurrency_ShouldRejectUpdate()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        Assert.Throws<InvalidOperationException>(() =>
            financialPeriod.UpdateExpenseDetail(expense.Id, detail.Id, "Dinner", Money.Create(20m, Currency.Usd), new DateOnly(2026, 8, 6), null));

        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(15_000m, detail.ActualAmount.Amount);
        Assert.Equal(15_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public void UpdateExpenseDetail_WhenPeriodIsClosed_ShouldThrowFinancialPeriodClosedException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        financialPeriod.EnterExpense(expense.Id);
        financialPeriod.Close();

        Assert.Throws<FinancialPeriodClosedException>(() =>
            financialPeriod.UpdateExpenseDetail(expense.Id, detail.Id, "Dinner", Money.Create(20_000m, Currency.Clp), new DateOnly(2026, 8, 6), null));
    }

    [Fact]
    public void RemoveExpenseDetail_WhenPeriodIsClosed_ShouldThrowFinancialPeriodClosedException()
    {
        var financialPeriod = CreateFinancialPeriod();
        var expense = CreateExpense();

        financialPeriod.AddExpense(expense);

        var detail = CreateDetail("Lunch", new DateOnly(2026, 8, 5));
        financialPeriod.AddExpenseDetail(expense.Id, detail);

        financialPeriod.EnterExpense(expense.Id);
        financialPeriod.Close();

        Assert.Throws<FinancialPeriodClosedException>(() =>
            financialPeriod.RemoveExpenseDetail(expense.Id, detail.Id));

        Assert.Single(expense.Details);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ExpenseDetailOperation_WhenExpenseDoesNotExist_ShouldThrowExpenseNotFoundException(bool update)
    {
        var financialPeriod = CreateFinancialPeriod();
        var expenseId = Guid.NewGuid();
        var detailId = Guid.NewGuid();

        if (update)
        {
            Assert.Throws<ExpenseNotFoundInFinancialPeriodException>(() =>
                financialPeriod.UpdateExpenseDetail(expenseId, detailId, "Lunch", Money.Create(15_000m, Currency.Clp), new DateOnly(2026, 8, 5), null));
        }
        else
        {
            Assert.Throws<ExpenseNotFoundInFinancialPeriodException>(() =>
                financialPeriod.RemoveExpenseDetail(expenseId, detailId));
        }
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