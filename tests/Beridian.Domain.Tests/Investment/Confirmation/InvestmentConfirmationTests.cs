using Beridian.Domain.Common;
using Beridian.Domain.Investments;
using Beridian.Domain.Investments.Exceptions;

namespace Beridian.Domain.Tests.Investments.Confirmation;

public sealed class InvestmentConfirmationTests
{
    [Fact]
    public void Confirm_WithValidAmount_ShouldStoreActualAmountAndChangeStatus()
    {
        var investment = CreateInvestment();
        var actualAmount = Money.Create(220_000m, Currency.Clp);

        investment.Confirm(actualAmount);

        Assert.Equal(actualAmount, investment.ActualAmount);
        Assert.Equal(InvestmentStatus.Confirmed, investment.Status);
    }

    [Fact]
    public void Confirm_WithNegativeAmount_ShouldThrowArgumentOutOfRangeException()
    {
        var investment = CreateInvestment();
        var actualAmount = Money.Create(-1m, Currency.Clp);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => investment.Confirm(actualAmount));

        Assert.Equal("actualAmount", exception.ParamName);
    }

    [Fact]
    public void Confirm_WhenAlreadyConfirmed_ShouldThrowInvalidOperationException()
    {
        var investment = CreateInvestment();

        investment.Confirm(Money.Create(220_000m, Currency.Clp));

        Assert.Throws<InvestmentAlreadyConfirmedException>(() => investment.Confirm(Money.Create(230_000m, Currency.Clp)));
    }

    private static Investment CreateInvestment()
    {
        return Investment.Create("Monthly Savings", Money.Create(250_000m, Currency.Clp));
    }
}