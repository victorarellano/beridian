namespace Beridian.Application.FinancialPeriods.GenerateNextFinancialPeriod;

public sealed record GenerateNextFinancialPeriodResult(Guid FinancialPeriodId, int Year, int Month);