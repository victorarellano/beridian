using Beridian.Application.Expenses.EnterExpense;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Application.Tests.Expenses.EnterExpense;

public sealed class EnterExpenseHandlerTest
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddEnterExpenseAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = RecurringExpense.Create("Luz", Money.Create(10_000m, Currency.Clp));

        financialPeriod.AddExpense(expense);
        repository.Seed(financialPeriod);

        var handler = new EnterExpenseHandler(repository);

        var command = new EnterExpenseCommand(financialPeriod.Id, expense.Id, 12_000m);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var updatedExpense = repository.UpdatedFinancialPeriod.Expenses.Single(x => x.Id == expense.Id);

        Assert.Equal(expense.Id, result.expenseId);
        Assert.Equal(12_000m, updatedExpense.ActualAmount.Amount);
        Assert.Equal(ExpenseStatus.Entered, updatedExpense.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenEnterExpenseDoesNotBelongToPeriod_ShouldThrowInvalidOperationExceptioForEnterExpense()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseHandler(repository);

        var command = new EnterExpenseCommand(financialPeriod.Id, Guid.NewGuid(), 12_000m);

        //Act & Assert

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodoIsClosed_ShouldThrowInvalidOperationExceptioForEnterExpense()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = RecurringExpense.Create(
            "Electricity",
            Money.Create(50_000m, Currency.Clp));

        financialPeriod.AddExpense(expense);
        financialPeriod.EnterExpense(
            expense.Id,
            Money.Create(48_000m, Currency.Clp));

        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseHandler(repository);
        var command = new EnterExpenseCommand(financialPeriod.Id, Guid.NewGuid(), 12_000m);

        //Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodCannotBeClosedException>(() => handler.HandleAsync(command));

        Assert.Equal(48_000m, expense.ActualAmount.Amount);
        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenExpenseHasDetails_ShouldRejectDirectActualAmount()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create(
            "Personal Expenses",
            Currency.Clp);

        financialPeriod.AddExpense(expense);

        financialPeriod.AddExpenseDetail(
            expense.Id,
            ExpenseDetail.Create(
                "Lunch",
                Money.Create(15_000m, Currency.Clp),
                new DateOnly(2026, 8, 12)));

        repository.Seed(financialPeriod);

        var handler = new EnterExpenseHandler(repository);

        var command = new EnterExpenseCommand(
            financialPeriod.Id,
            expense.Id,
            20_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command));

        Assert.Equal(ExpenseStatus.Created, expense.Status);
        Assert.Null(repository.UpdatedFinancialPeriod);
    }

}