using Beridian.Domain.Events;

namespace Beridian.Application.Abstractions.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}