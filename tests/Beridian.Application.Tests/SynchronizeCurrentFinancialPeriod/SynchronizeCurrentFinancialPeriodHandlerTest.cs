using Beridian.Application.Exceptions;
using Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.FinancialPeriods.SynchronizeCurrentFinancialPeriod;

public sealed class SynchronizeCurrentFinancialPeriodHandlerTest
{
    [Fact]
    public async Task HandleAsync_WhenCurrentFinancialPeriodExists_ShouldReturnExistingPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();
        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 9));
        repository.Seed(financialPeriod);

        var handler = new SynchronizeCurrentFinancialPeriodHandler(repository);
        var command = new SynchronizeCurrentFinancialPeriodCommand(2026, 9);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(financialPeriod.Id, result.FinancialPeriodId);
        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.False(result.WasCreated);
    }

    [Fact]
    public async Task HandleAsync_WhenNoFinancialHistoryExists_ShouldCreateCurrentFinancialPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();
        var handler = new SynchronizeCurrentFinancialPeriodHandler(repository);

        var command = new SynchronizeCurrentFinancialPeriodCommand(2026, 9);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.True(result.WasCreated);

        var financialPeriod = await repository.GetByPeriodAsync(Period.Create(2026, 9));

        Assert.NotNull(financialPeriod);
        Assert.Equal(result.FinancialPeriodId, financialPeriod.Id);
    }

    [Fact]
    public async Task HandleAsync_WhenHistoryExistsButCurrentPeriodDoesNotExist_ShouldThrowCurrentFinancialPeriodNotAvailableException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();
        var previousFinancialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        repository.Seed(previousFinancialPeriod);

        var handler = new SynchronizeCurrentFinancialPeriodHandler(repository);
        var command = new SynchronizeCurrentFinancialPeriodCommand(2026, 9);

        // Act
        var action = async () => await handler.HandleAsync(command);

        // Assert
        var exception = await Assert.ThrowsAsync<CurrentFinancialPeriodNotAvailableException>(action);
        Assert.Equal(2026, exception.Year);
        Assert.Equal(9, exception.Month);
    }    
}