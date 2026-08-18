using Beridian.Domain.FinancialPeriods;

namespace Beridian.Domain.Tests.FinancialPeriods;

public sealed class PeriodTests
{
    [Fact]
    public void Create_WithValidYearAndMonth_ShouldCreatePeriod()
    {
        var period = Period.Create(2026, 8);

        Assert.Equal(2026, period.Year);
        Assert.Equal(8, period.Month);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidYear_ShouldThrowArgumentOutOfRangeException(
        int year)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Period.Create(year, 8));

        Assert.Equal("year", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    [InlineData(-1)]
    public void Create_WithInvalidMonth_ShouldThrowArgumentOutOfRangeException(
        int month)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Period.Create(2026, month));

        Assert.Equal("month", exception.ParamName);
    }

    [Fact]
    public void Contains_WithDateInSamePeriod_ShouldReturnTrue()
    {
        var period = Period.Create(2026, 8);
        var date = new DateOnly(2026, 8, 15);

        var result = period.Contains(date);

        Assert.True(result);
    }

    [Fact]
    public void Contains_WithDateOutsidePeriod_ShouldReturnFalse()
    {
        var period = Period.Create(2026, 8);
        var date = new DateOnly(2026, 9, 1);

        var result = period.Contains(date);

        Assert.False(result);
    }

    [Fact]
    public void Next_WhenCurrentMonthIsNotDecember_ShouldReturnFollowingMonth()
    {
        var period = Period.Create(2026, 8);

        var nextPeriod = period.Next();

        Assert.Equal(2026, nextPeriod.Year);
        Assert.Equal(9, nextPeriod.Month);
    }

    [Fact]
    public void Next_WhenCurrentMonthIsDecember_ShouldReturnJanuaryOfFollowingYear()
    {
        var period = Period.Create(2026, 12);

        var nextPeriod = period.Next();

        Assert.Equal(2027, nextPeriod.Year);
        Assert.Equal(1, nextPeriod.Month);
    }
}