using Beridian.Domain.Events;

namespace Beridian.Domain.FinancialPeriods.Events;

public sealed record FinancialPeriodGenerated(Guid SourceFinancialPeriodId, Guid GeneratedFinancialPeriodId, Period GeneratedPeriod) : IDomainEvent;