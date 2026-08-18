using Beridian.Application.Expenses.AddDiscretionaryExpense;
using Beridian.Application.Expenses.AddExpenseDetail;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Application.Tests.Expenses.AddExpenseDetail;

public sealed class AddExpenseDetailHandlerTest
{
    [Fact]
    public async Task HandleAsync_WithCommandValid_ShouldAddExpenseDetailAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();
        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create(
                "Mis Gastos",
                Currency.Clp);

        financialPeriod.AddExpense(expense);

        repository.Seed(financialPeriod);

        var handler = new AddExpenseDetailHandler(repository);

        var command = new AddExpenseDetailCommand(
            financialPeriod.Id,
            expense.Id,
            "Lunch",
            15_000m,
            new DateOnly(2026, 8, 12));

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var detail = Assert.Single(expense.Details);

        Assert.Equal("Lunch", detail.Description);
        Assert.Equal(15_000m, detail.ActualAmount.Amount);
        Assert.Equal(detail.Id, result.ExpenseDetailId);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowInvalidOperationExceptionExpenseDetail()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddExpenseDetailHandler(repository);

        var command = new AddExpenseDetailCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Lunch",
            15_000m,
            new DateOnly(2026, 8, 12));

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenExpenseDetailDoesNotBelongToPeriod_ShouldThrowInvalidOperationExpenseDetail()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new AddExpenseDetailHandler(repository);
        var command = new AddExpenseDetailCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Lunch",
            15_000m);

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClose_ShouldThrowInvalidOperationExpenseDetail()
    {
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddExpenseDetailHandler(repository);
        var command = new AddExpenseDetailCommand(financialPeriod.Id, Guid.NewGuid(), "Lunch", 15_000m);

        await Assert.ThrowsAsync<FinancialPeriodCannotBeClosedException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);

    }
}