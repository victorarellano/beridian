using Beridian.Domain.Common;
using Beridian.Domain.Investments;

namespace Beridian.Domain.Tests.Investments.Creation;

public sealed class InvestmentCreationTests
{
    [Fact]
    public void Create_WithValidValues_ShouldCreateInvestmentInCreatedStatus()
    {
        var plannedAmount = Money.Create(250_000m, Currency.Clp);

        var investment = Investment.Create("Monthly Savings", plannedAmount);

        Assert.NotEqual(Guid.Empty, investment.Id);
        Assert.Equal("Monthly Savings", investment.Name);
        Assert.Equal(plannedAmount, investment.PlannedAmount);
        Assert.Equal(Money.Zero(Currency.Clp), investment.ActualAmount);
        Assert.Equal(InvestmentStatus.Created, investment.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string name)
    {
        var plannedAmount = Money.Create(250_000m, Currency.Clp);

        var exception = Assert.Throws<ArgumentException>(
            () => Investment.Create(name, plannedAmount));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_WithNegativePlannedAmount_ShouldThrowArgumentOutOfRangeException()
    {
        var plannedAmount = Money.Create(-1m, Currency.Clp);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Investment.Create(
                "Monthly Savings",
                plannedAmount));

        Assert.Equal("plannedAmount", exception.ParamName);
    }
}