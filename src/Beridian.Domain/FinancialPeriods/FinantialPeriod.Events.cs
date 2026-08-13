using Beridian.Domain.Events;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    internal void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}