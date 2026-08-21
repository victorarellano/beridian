using Beridian.Application.Expenses.AddDiscretionaryExpense;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Application.Tests.Expenses.AddDiscretionaryExpense;

public sealed class AddDiscretionaryExpenseHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddDiscretionaryExpenseAndPersistPeriod()
    {
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new AddDiscretionaryExpenseHandler(repository);

        var command = new AddDiscretionaryExpenseCommand(financialPeriod.Id, "Personal Expenses");

        var result = await handler.HandleAsync(command);

        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var expense = Assert.IsType<DiscretionaryExpense>(Assert.Single(repository.UpdatedFinancialPeriod.Expenses));

        Assert.Equal("Personal Expenses", expense.Name);
        Assert.Equal(0m, expense.PlannedAmount.Amount);
        Assert.Equal(0m, expense.ActualAmount.Amount);
        Assert.Equal(expense.Id, result.ExpenseId);
    }

    [Fact]
    public async Task HandleAsync_WhenPeriodDoesNotExist_ShouldThrowInvalidOperationExceptionByDiscretionaryExpense()
    {
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddDiscretionaryExpenseHandler(repository);

        var command = new AddDiscretionaryExpenseCommand(Guid.NewGuid(), "Personal Expenses");

        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationExceptionForDiscretionaryExpense()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddDiscretionaryExpenseHandler(repository);
        var command = new AddDiscretionaryExpenseCommand(financialPeriod.Id, "Personal Expenses");

        //Act & Assert

        await Assert.ThrowsAsync<FinancialPeriodClosedException>(() => handler.HandleAsync(command));
        Assert.Empty(financialPeriod.Expenses);
        Assert.Null(repository.UpdatedFinancialPeriod);
    }
}