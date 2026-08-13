using Beridian.Application.Abstractions.Events;
using Beridian.Domain.Events;

namespace Beridian.Application.Tests.TestDoubles;

internal sealed class FakeDomainEventDispatcher
    : IDomainEventDispatcher
{
    private readonly List<IDomainEvent> _dispatchedEvents = [];

    public IReadOnlyCollection<IDomainEvent> DispatchedEvents => _dispatchedEvents.AsReadOnly();

    public Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        _dispatchedEvents.AddRange(domainEvents);

        return Task.CompletedTask;
    }
}