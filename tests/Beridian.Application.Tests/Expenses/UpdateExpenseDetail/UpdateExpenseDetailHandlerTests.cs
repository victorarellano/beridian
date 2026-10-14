using Beridian.Application.Expenses.UpdateExpenseDetail;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.Expenses.UpdateExpenseDetail;

public sealed class UpdateExpenseDetailHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldUpdateDetailAndPersistPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));

        var expense = DiscretionaryExpense.Create("Personal Expenses", Currency.Clp);

        financialPeriod.AddExpense(expense);

        var detail = ExpenseDetail.Create("Lunch", Money.Create(15_000m, Currency.Clp), new DateOnly(2026, 8, 5));

        financialPeriod.AddExpenseDetail(expense.Id, detail);

        repository.Seed(financialPeriod);

        var handler = new UpdateExpenseDetailHandler(repository);

        var command = new UpdateExpenseDetailCommand(financialPeriod.Id, expense.Id, detail.Id, "Dinner", 25_000m, new DateOnly(2026, 8, 10), 30_000m);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);
        Assert.Same(financialPeriod, repository.UpdatedFinancialPeriod);

        Assert.Equal(detail.Id, result.ExpenseDetailId);
        Assert.Equal("Dinner", detail.Description);
        Assert.Equal(25_000m, detail.ActualAmount.Amount);
        Assert.Equal(30_000m, detail.PlannedAmount?.Amount);
        Assert.Equal(25_000m, expense.ActualAmount.Amount);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowFinancialPeriodNotFoundException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new UpdateExpenseDetailHandler(repository);

        var command = new UpdateExpenseDetailCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Dinner", 25_000m);

        // Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }
}