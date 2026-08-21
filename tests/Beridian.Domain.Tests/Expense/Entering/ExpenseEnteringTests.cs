using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Domain.Tests.Expenses.Entering;

public sealed class ExpenseEnteringTests
{
    [Fact]
    public void Enter_WithoutDetails_ShouldStoreActualAmountAndChangeStatus()
    {
        var expense = CreateExpense();
        var actualAmount = Money.Create(45000m, Currency.Clp);

        expense.Enter(actualAmount);

        Assert.Equal(actualAmount, expense.ActualAmount);
        Assert.Equal(ExpenseStatus.Entered, expense.Status);
    }

    [Fact]
    public void Enter_WithDetails_ShouldUseSumOfDetailAmountsAndChangeStatus()
    {
        var expense = CreateExpense();

        expense.AddDetail(
            ExpenseDetail.Create("Lunch", Money.Create(15000m, Currency.Clp), new DateOnly(2026, 8, 5)));

        expense.AddDetail(
            ExpenseDetail.Create("Lunch", Money.Create(10000m, Currency.Clp), new DateOnly(2026, 8, 6)));

        expense.Enter();

        Assert.Equal(25000m, expense.ActualAmount.Amount);
        Assert.Equal(ExpenseStatus.Entered, expense.Status);
    }

    [Fact]
    public void Enter_WithoutDetailsAndWithoutActualAmount_ShouldThrowInvalidOperationException()
    {
        var expense = CreateExpense();

        Assert.Throws<ExpenseHasDetailsException>(() => expense.Enter());
    }

    [Fact]
    public void Enter_WithDetailsAndDirectActualAmount_ShouldThrowInvalidOperationException()
    {
        var expense = CreateExpense();

        expense.AddDetail(
            ExpenseDetail.Create("Lunch", Money.Create(15000m, Currency.Clp), new DateOnly(2026, 8, 5)));

        Assert.Throws<ExpenseHasDetailsException>(() => expense.Enter(Money.Create(20000m, Currency.Clp)));
    }

    [Fact]
    public void Enter_WhenAlreadyEntered_ShouldThrowExpenseAlreadyEnteredException()
    {
        var expense = CreateExpense();

        expense.Enter(Money.Create(45000m, Currency.Clp));

        Assert.Throws<ExpenseAlreadyEnteredException>(() => expense.Enter(Money.Create(46000m, Currency.Clp)));
    }

    [Fact]
    public void Create_WithValidValues_ShouldCreateExpenseDetail()
    {
        var actualAmount = Money.Create(15000m, Currency.Clp);

        var detail = ExpenseDetail.Create("Lunch", actualAmount, new DateOnly(2026, 8, 5));

        Assert.NotEqual(Guid.Empty, detail.Id);
        Assert.Equal(new DateOnly(2026, 8, 5), detail.TransactionDate);
        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(actualAmount, detail.ActualAmount);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidDescription_ShouldThrowArgumentException(string description)
    {
        var actualAmount = Money.Create(15000m, Currency.Clp);

        var exception = Assert.Throws<ArgumentException>(
            () => ExpenseDetail.Create(description, actualAmount, new DateOnly(2026, 8, 5)));

        Assert.Equal("description", exception.ParamName);
    }

    private static Expense CreateExpense()
    {
        return RecurringExpense.Create("Electricity", Money.Create(50000m, Currency.Clp));
    }
}