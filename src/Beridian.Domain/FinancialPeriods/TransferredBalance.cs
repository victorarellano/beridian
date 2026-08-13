using Beridian.Domain.Common;

namespace Beridian.Domain.FinancialPeriods;

public sealed record TransferredBalance
{
    public Money Amount { get; }

    private TransferredBalance(Money amount)
    {
        Amount = amount;
    }

    public static TransferredBalance Create(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        return new TransferredBalance(amount);
    }

    public static TransferredBalance Zero(Currency currency)
    {
        return Create(Money.Zero(currency));
    }
}