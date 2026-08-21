namespace Beridian.Domain.Investments.Exceptions;

public sealed class InvestmentAlreadyConfirmedException
    : Exception
{
    public Guid InvestmentId { get; }

    public InvestmentAlreadyConfirmedException(Guid investmentId)
        : base($"Investment '{investmentId}' has already been confirmed.")
    {
        InvestmentId = investmentId;
    }
}