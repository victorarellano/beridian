using Beridian.Application.Abstractions.Events;
using Beridian.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Beridian.Infrastructure.Events;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(
        ILogger<DomainEventDispatcher> logger)
    {
        _logger = logger;
    }

    public Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvents);

        foreach (var domainEvent in domainEvents)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation(
                "Domain event dispatched: {DomainEventType}",
                domainEvent.GetType().Name);
        }

        return Task.CompletedTask;
    }
}