using Beridian.Application.Expenses.AddFixedTermExpense;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Application.Tests.Expenses.AddFixedTermExpense;

public sealed class AddFixedTermExpenseHandlerTest
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddFixedTermExpenseAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new AddFixedTermExpenseHandler(repository);
        var command = new AddFixedTermExpenseCommand(financialPeriod.Id, "Celular 8de12", Money.Create(54000, Currency.Clp), 8, 12);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var expense = Assert.IsType<FixedTermExpense>(Assert.Single(repository.UpdatedFinancialPeriod.Expenses));

        Assert.Equal("Celular 8de12", expense.Name);

        Assert.Equal(54000, expense.PlannedAmount.Amount);

        Assert.Equal(0m, expense.ActualAmount.Amount);

        Assert.Equal(expense.Id, result.ExpenseId);
    }

    [Fact]
    public async Task HandleAsync_WhenPeriodDoesNotExist_ShouldThrowInvalidOperationExceptionForFixedTermExpense()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddFixedTermExpenseHandler(repository);
        var command = new AddFixedTermExpenseCommand(Guid.NewGuid(), "Celular 8de12", Money.Create(54000, Currency.Clp), 8, 12);

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationExceptionForFixedTermExpense()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddFixedTermExpenseHandler(repository);
        var command = new AddFixedTermExpenseCommand(financialPeriod.Id, "Celular 8de12", Money.Create(54000, Currency.Clp), 8, 12);

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodCannotBeClosedException>(() => handler.HandleAsync(command));
        Assert.Empty(financialPeriod.Expenses);
        Assert.Null(repository.UpdatedFinancialPeriod);
    }

}