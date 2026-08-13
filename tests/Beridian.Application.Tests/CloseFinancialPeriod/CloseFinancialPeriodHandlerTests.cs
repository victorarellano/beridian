using Beridian.Application.FinancialPeriods.CloseFinancialPeriod;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Events;

namespace Beridian.Application.Tests.FinancialPeriods.CloseFinancialPeriod;

public sealed class CloseFinancialPeriodHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithExistingClosablePeriod_ShouldCloseAndPersistPeriod()
    {
        var repository = new FakeFinancialPeriodRepository();
        var eventDispatcher = new FakeDomainEventDispatcher();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new CloseFinancialPeriodHandler(repository, eventDispatcher);

        var command = new CloseFinancialPeriodCommand(financialPeriod.Id);

        var result = await handler.HandleAsync(command);

        Assert.NotNull(repository.UpdatedFinancialPeriod);

        Assert.Equal(FinancialPeriodStatus.Closed, repository.UpdatedFinancialPeriod.Status);

        Assert.Equal(financialPeriod.Id, result.FinancialPeriodId);

        var dispatchedEvent = Assert.IsType<FinancialPeriodClosed>(Assert.Single(eventDispatcher.DispatchedEvents));

        Assert.Equal(result.FinancialPeriodId, dispatchedEvent.FinancialPeriodId);
        Assert.Empty(financialPeriod.DomainEvents);
    }

    [Fact]
    public async Task HandleAsync_WhenPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var repository = new FakeFinancialPeriodRepository();
        var eventDispatcher = new FakeDomainEventDispatcher();

        var handler = new CloseFinancialPeriodHandler(repository, eventDispatcher);

        var command = new CloseFinancialPeriodCommand(Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }
}