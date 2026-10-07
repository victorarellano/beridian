namespace Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;

public sealed record SynchronizeCurrentFinancialPeriodResult(Guid FinancialPeriodId, int Year, int Month, bool WasCreated);