using Beridian.Domain.Common;
using Beridian.Domain.Incomes;
using Beridian.Domain.Incomes.Exceptions;

namespace Beridian.Domain.Tests.Incomes.Entering;

public sealed class IncomeEnteringTests
{
    [Fact]
    public void Enter_WithValidActualAmount_ShouldStoreAmountAndChangeStatus()
    {
        var income = CreateIncome();
        var actualAmount = Money.Create(1_520_000m, Currency.Clp);

        income.Enter(actualAmount);

        Assert.Equal(actualAmount, income.ActualAmount);
        Assert.Equal(IncomeStatus.Entered, income.Status);
    }

    [Fact]
    public void Enter_WithNegativeActualAmount_ShouldThrowArgumentOutOfRangeException()
    {
        var income = CreateIncome();
        var actualAmount = Money.Create(-1m, Currency.Clp);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => income.Enter(actualAmount));

        Assert.Equal("actualAmount", exception.ParamName);
    }

    [Fact]
    public void Enter_WhenAlreadyEntered_ShouldThrowInvalidOperationException()
    {
        var income = CreateIncome();

        income.Enter(Money.Create(1_500_000m, Currency.Clp));

        Assert.Throws<IncomeAlreadyEnteredException>(
            () => income.Enter(Money.Create(1_520_000m, Currency.Clp)));
    }

    private static Income CreateIncome()
    {
        return Income.Create("Salary", Money.Create(1_500_000m, Currency.Clp));
    }
}