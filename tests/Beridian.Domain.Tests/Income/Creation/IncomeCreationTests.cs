using Beridian.Domain.Common;
using Beridian.Domain.Incomes;

namespace Beridian.Domain.Tests.Incomes.Creation;

public sealed class IncomeCreationTests
{
    [Fact]
    public void Create_WithValidValues_ShouldCreateIncomeInCreatedStatus()
    {
        var plannedAmount = Money.Create(1_500_000m, Currency.Clp);
        var income = Income.Create("Salary", plannedAmount);

        Assert.NotEqual(Guid.Empty, income.Id);
        Assert.Equal("Salary", income.Name);
        Assert.Equal(plannedAmount, income.PlannedAmount);
        Assert.Equal(0m, income.ActualAmount.Amount);
        Assert.Equal(Currency.Clp, income.ActualAmount.Currency);
        Assert.Equal(IncomeStatus.Created, income.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string name)
    {
        var plannedAmount = Money.Create(1_500_000m, Currency.Clp);

        var exception = Assert.Throws<ArgumentException>(
            () => Income.Create(name, plannedAmount));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_WithNegativePlannedAmount_ShouldThrowArgumentOutOfRangeException()
    {
        var plannedAmount = Money.Create(-1m, Currency.Clp);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Income.Create("Salary", plannedAmount));

        Assert.Equal("plannedAmount", exception.ParamName);
    }
}