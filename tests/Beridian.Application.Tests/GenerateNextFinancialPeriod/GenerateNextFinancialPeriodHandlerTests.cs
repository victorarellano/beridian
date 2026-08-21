using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.FinancialPeriods.GenerateNextFinancialPeriod;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Events;
using Beridian.Domain.Services;

namespace Beridian.Application.Tests.FinancialPeriods.CreateFinancialPeriod;

public sealed class GenerateNextFinancialPeriodHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithExistingPeriod_ShouldGenerateAndPersistNextPeriod()
    {
        var repository = new FakeFinancialPeriodRepository();
        var eventDispatcher = new FakeDomainEventDispatcher();

        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(currentPeriod);

        var generator = new FinancialPeriodGenerator();

        var handler = new GenerateNextFinancialPeriodHandler(repository, generator, eventDispatcher);
        var command = new GenerateNextFinancialPeriodCommand(currentPeriod.Id);

        var result = await handler.HandleAsync(command);

        Assert.NotNull(repository.AddedFinancialPeriod);

        Assert.Equal(2026, repository.AddedFinancialPeriod.Period.Year);

        Assert.Equal(9, repository.AddedFinancialPeriod.Period.Month);

        Assert.Equal(repository.AddedFinancialPeriod.Id, result.FinancialPeriodId);

        var dispatchedEvent = Assert.IsType<FinancialPeriodGenerated>(Assert.Single(eventDispatcher.DispatchedEvents));

        Assert.Equal(result.FinancialPeriodId, dispatchedEvent.GeneratedFinancialPeriodId);
        Assert.Empty(currentPeriod.DomainEvents);        
    }

    [Fact]
    public async Task HandleAsync_WhenPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var repository = new FakeFinancialPeriodRepository();
        var eventDispatcher = new FakeDomainEventDispatcher();

        var generator = new FinancialPeriodGenerator();

        var handler = new GenerateNextFinancialPeriodHandler(repository, generator, eventDispatcher);

        var command = new GenerateNextFinancialPeriodCommand(Guid.NewGuid());

        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.AddedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenNextFinancialPeriodAlreadyExists_ShouldThrowFinancialPeriodAlreadyExistsException()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();
        var eventDispatcher = new FakeDomainEventDispatcher();

        var currentPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        var existingNextPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 9));

        repository.Seed(currentPeriod);
        repository.Seed(existingNextPeriod);        

        var generator = new FinancialPeriodGenerator();

        var handler = new GenerateNextFinancialPeriodHandler(repository, generator, eventDispatcher);
        var command = new GenerateNextFinancialPeriodCommand(currentPeriod.Id);

        //Act && Assert
        var exception = await Assert.ThrowsAsync<FinancialPeriodAlreadyExistsException>( () => handler.HandleAsync(command) );

        Assert.Equal(2026, exception.Year);
        Assert.Equal(9, exception.Month);

        Assert.Null(repository.AddedFinancialPeriod);
        Assert.Empty(eventDispatcher.DispatchedEvents);
    }
}