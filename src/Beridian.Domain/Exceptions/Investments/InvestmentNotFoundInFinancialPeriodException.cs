namespace Beridian.Domain.Investments.Exceptions;

public sealed class InvestmentNotFoundInFinancialPeriodException : Exception
{
    public Guid FinancialPeriodId { get; }

    public Guid InvestmentId { get; }

    public InvestmentNotFoundInFinancialPeriodException(Guid financialPeriodId, Guid investmentId)
        : base($"Investment '{investmentId}' was not found in financial period '{financialPeriodId}'.")
    {
        FinancialPeriodId = financialPeriodId;
        InvestmentId = investmentId;
    }
}