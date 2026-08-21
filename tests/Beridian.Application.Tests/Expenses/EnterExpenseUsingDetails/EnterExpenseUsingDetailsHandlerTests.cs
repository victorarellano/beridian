using Beridian.Application.Expenses.EnterExpenseUsingDetails;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Expenses.Exceptions;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Application.Tests.Expenses.EnterExpenseUsingDetails;

public sealed class EnterExpenseUsingDetailsHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldEnterExpenseUsingDetailsAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Mis Gastos", Currency.Clp);

        financialPeriod.AddExpense(expense);

        financialPeriod.AddExpenseDetail(
            expense.Id,
            ExpenseDetail.Create(
                "Lunch",
                Money.Create(5_000m, Currency.Clp),
                new DateOnly(2026, 8, 12)));

        financialPeriod.AddExpenseDetail(
            expense.Id,
            ExpenseDetail.Create(
                "Lunch",
                Money.Create(4_500m, Currency.Clp),
                new DateOnly(2026, 8, 13)));                       

        

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseUsingDetailsHandler(repository);

        var command = new EnterExpenseUsingDetailsCommand(financialPeriod.Id, expense.Id);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var storedExpense = repository.UpdatedFinancialPeriod.Expenses.Single(x => x.Id == expense.Id);

        Assert.Equal(expense.Id, result.ExpenseId);
        Assert.Equal(9_500m, storedExpense.ActualAmount.Amount);
        Assert.Equal(ExpenseStatus.Entered, storedExpense.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var expense = DiscretionaryExpense.Create("Mis Gastos", Currency.Clp);

        var handler = new EnterExpenseUsingDetailsHandler(repository);
        var command = new EnterExpenseUsingDetailsCommand(Guid.NewGuid(), expense.Id);

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClose_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Mis Gastos", Currency.Clp);

        financialPeriod.AddExpense(expense);

        financialPeriod.AddExpenseDetail(
            expense.Id,
            ExpenseDetail.Create(
                "Lunch",
                Money.Create(5_000m, Currency.Clp),
                new DateOnly(2026, 8, 12)));

        financialPeriod.EnterExpense(expense.Id);
        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseUsingDetailsHandler(repository);

        var command = new EnterExpenseUsingDetailsCommand(financialPeriod.Id, expense.Id);

        // Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodClosedException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);

        var storedExpense = financialPeriod.Expenses.Single(x => x.Id == expense.Id);

        Assert.Equal(5_000m, storedExpense.ActualAmount.Amount);
        Assert.Equal(ExpenseStatus.Entered, storedExpense.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenExpenseDetailDoesNotBelongToPeriod_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseUsingDetailsHandler(repository);

        var command = new EnterExpenseUsingDetailsCommand(financialPeriod.Id, Guid.NewGuid());

        //Act & Assert

        await Assert.ThrowsAsync<ExpenseNotFoundInFinancialPeriodException>(() => handler.HandleAsync(command));
        Assert.Null(repository.UpdatedFinancialPeriod);        
    }

    [Fact]
    public async Task HandleAsync_WhenExpenseDetailHasNoDetails_ShouldRejectExpense()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Mis Gastos", Currency.Clp);

        financialPeriod.AddExpense(expense);

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseUsingDetailsHandler(repository);
        var command = new EnterExpenseUsingDetailsCommand(financialPeriod.Id, expense.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ExpenseHasDetailsException>(() => handler.HandleAsync(command));

        var storedExpense = financialPeriod.Expenses.Single(x => x.Id == expense.Id);

        Assert.Null(repository.UpdatedFinancialPeriod);
        Assert.Equal(ExpenseStatus.Created, storedExpense.Status);        
    }
}