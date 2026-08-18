namespace Beridian.Application.FinancialPeriods.Exceptions;

public sealed class FinancialPeriodNotFoundException : Exception
{
    public Guid FinancialPeriodId { get; }

    public FinancialPeriodNotFoundException(Guid financialPeriodId)
        : base($"Financial period '{financialPeriodId}' was not found.")
    {
        FinancialPeriodId = financialPeriodId;
    }
}