using Beridian.Domain.FinancialPeriods;

namespace Beridian.Domain.Tests.FinancialPeriods;

public sealed class FinancialPeriodTests
{
    [Fact]
    public void Create_WithValidPeriod_ShouldCreateOpenFinancialPeriod()
    {
        var period = Period.Create(2026, 8);

        var financialPeriod = FinancialPeriod.CreateInitial(period);

        Assert.NotEqual(Guid.Empty, financialPeriod.Id);
        Assert.Equal(period, financialPeriod.Period);
        Assert.Equal(
            FinancialPeriodStatus.Open,
            financialPeriod.Status);
    }

    [Fact]
    public void Create_WithNullPeriod_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => FinancialPeriod.CreateInitial(null!));

        Assert.Equal("period", exception.ParamName);
    }
}