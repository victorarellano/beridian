using Beridian.Application.Expenses.RemoveExpenseDetail;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.Expenses.Exceptions;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.Expenses.RemoveExpenseDetail;

public sealed class RemoveExpenseDetailHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldRemoveDetailAndPersistPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Personal Expenses", Currency.Clp);

        financialPeriod.AddExpense(expense);

        var detail = ExpenseDetail.Create("Lunch", Money.Create(15_000m, Currency.Clp), new DateOnly(2026, 8, 5));

        financialPeriod.AddExpenseDetail(expense.Id, detail);

        repository.Seed(financialPeriod);

        var handler = new RemoveExpenseDetailHandler(repository);

        var command = new RemoveExpenseDetailCommand(financialPeriod.Id, expense.Id, detail.Id);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Same(financialPeriod, repository.UpdatedFinancialPeriod);

        Assert.Equal(detail.Id, result.ExpenseDetailId);
        Assert.Empty(expense.Details);
        Assert.Equal(0m, expense.ActualAmount.Amount);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowFinancialPeriodNotFoundException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new RemoveExpenseDetailHandler(repository);

        var command = new RemoveExpenseDetailCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenDetailDoesNotExist_ShouldThrowExpenseDetailNotFoundException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Personal Expenses", Currency.Clp);

        financialPeriod.AddExpense(expense);

        repository.Seed(financialPeriod);

        var handler = new RemoveExpenseDetailHandler(repository);

        var command = new RemoveExpenseDetailCommand(financialPeriod.Id, expense.Id, Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ExpenseDetailNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }
}