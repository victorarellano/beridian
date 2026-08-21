namespace Beridian.Domain.FinancialPeriods.Exceptions;

public sealed class FinancialPeriodClosedException : InvalidOperationException
{
    public Guid FinancialPeriodId { get; }

    public FinancialPeriodClosedException(Guid financialPeriodId)
        : base("A closed financial period cannot be modified.")
    {
        FinancialPeriodId = financialPeriodId;
    }
}