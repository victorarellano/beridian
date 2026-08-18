using Beridian.Domain.Common;

namespace Beridian.Domain.Tests.Common;

public sealed class MoneyTests
{
    [Fact]
    public void Create_WithValidAmountAndCurrency_ShouldCreateMoney()
    {
        var money = Money.Create(15000m, Currency.Clp);

        Assert.Equal(15000m, money.Amount);
        Assert.Equal(Currency.Clp, money.Currency);
    }

    [Fact]
    public void Zero_WithValidCurrency_ShouldCreateZeroMoney()
    {
        var money = Money.Zero(Currency.Clp);

        Assert.Equal(0m, money.Amount);
        Assert.Equal(Currency.Clp, money.Currency);
    }

    [Fact]
    public void Create_WithInvalidCurrency_ShouldThrowArgumentOutOfRangeException()
    {
        var invalidCurrency = (Currency)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Money.Create(15000m, invalidCurrency));

        Assert.Equal("currency", exception.ParamName);
    }

    [Fact]
    public void Add_WithSameCurrency_ShouldReturnCombinedMoney()
    {
        var first = Money.Create(10000m, Currency.Clp);
        var second = Money.Create(5000m, Currency.Clp);

        var result = first.Add(second);

        Assert.Equal(15000m, result.Amount);
        Assert.Equal(Currency.Clp, result.Currency);
    }

    [Fact]
    public void Add_WithNullMoney_ShouldThrowArgumentNullException()
    {
        var money = Money.Create(10000m, Currency.Clp);

        var exception = Assert.Throws<ArgumentNullException>(
            () => money.Add(null!));

        Assert.Equal("other", exception.ParamName);
    }

    [Fact]
    public void Subtract_WithSameCurrency_ShouldReturnDifference()
    {
        var first = Money.Create(20_000m, Currency.Clp);
        var second = Money.Create(5_000m, Currency.Clp);

        var result = first.Subtract(second);

        Assert.Equal(15_000m, result.Amount);
        Assert.Equal(Currency.Clp, result.Currency);
    }
}