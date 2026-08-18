namespace Beridian.Domain.Common;

public sealed record Money
{
    public decimal Amount { get; private set; }

    public Currency Currency { get; private set;}

    private Money()
    {
    }
    
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (!Enum.IsDefined(currency))
        {
            throw new ArgumentOutOfRangeException(
                nameof(currency),
                "Currency must be valid.");
        }

        return new Money(amount, currency);
    }

    public static Money Zero(Currency currency)
    {
        return Create(0m, currency);
    }

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Currency != other.Currency)
        {
            throw new InvalidOperationException(
                "Money values with different currencies cannot be added.");
        }

        return new Money(
            Amount + other.Amount,
            Currency);
    }

    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Currency != other.Currency)
        {
            throw new InvalidOperationException(
                "Money values with different currencies cannot be subtracted.");
        }

        return new Money(Amount - other.Amount, Currency);
    }
}