using Beridian.Application.FinancialPeriods.CreateFinancialPeriod;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.FinancialPeriods.CreateFinancialPeriod;

public sealed class CreateFinancialPeriodHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldCreateAndPersistFinancialPeriod()
    {
        var repository = new FakeFinancialPeriodRepository();

        var handler = new CreateFinancialPeriodHandler(repository);

        var command = new CreateFinancialPeriodCommand(2026, 8);

        var result = await handler.HandleAsync(command);

        Assert.NotNull(repository.AddedFinancialPeriod);

        Assert.Equal(FinancialPeriodStatus.Open, repository.AddedFinancialPeriod.Status);

        Assert.Equal(2026, repository.AddedFinancialPeriod.Period.Year);

        Assert.Equal(8, repository.AddedFinancialPeriod.Period.Month);

        Assert.Equal(repository.AddedFinancialPeriod.Id, result.FinancialPeriodId);

        Assert.Equal(2026, result.Year);
        Assert.Equal(8, result.Month);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidMonth_ShouldPropagateDomainValidation()
    {
        var repository = new FakeFinancialPeriodRepository();

        var handler = new CreateFinancialPeriodHandler(repository);

        var command = new CreateFinancialPeriodCommand(2026, 13);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => handler.HandleAsync(command));

        Assert.Null(repository.AddedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodAlreadyExists_ShouldThrowFinancialPeriodAlreadyExistsException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var existingFinancialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 9));

        repository.Seed(existingFinancialPeriod);

        var handler = new CreateFinancialPeriodHandler(repository);

        var command = new CreateFinancialPeriodCommand(2026, 9);

        // Act
        var action = async () => await handler.HandleAsync(command);

        // Assert
        var exception = await Assert.ThrowsAsync<FinancialPeriodAlreadyExistsException>(action);

        Assert.Equal(2026, exception.Year);
        Assert.Equal(9, exception.Month);
        Assert.Null(repository.AddedFinancialPeriod);
    }

}