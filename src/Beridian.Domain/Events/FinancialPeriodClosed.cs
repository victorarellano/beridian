using Beridian.Domain.Events;

namespace Beridian.Domain.FinancialPeriods.Events;

public sealed record FinancialPeriodClosed(Guid FinancialPeriodId, Period Period) : IDomainEvent;