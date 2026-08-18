using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Domain.Tests.FinancialPeriods.Balances;

public sealed class TransferredBalanceTests
{
    [Fact]
    public void CreateInitial_ShouldCreatePeriodWithZeroOpeningBalance()
    {
        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        Assert.Equal(Money.Zero(Currency.Clp), financialPeriod.OpeningBalance.Amount);
    }

    [Fact]
    public void Create_WithTransferredBalance_ShouldStoreOpeningBalance()
    {
        var openingBalance = TransferredBalance.Create(Money.Create(125_000m, Currency.Clp));

        var financialPeriod = FinancialPeriod.Create(Period.Create(2026, 9), openingBalance);

        Assert.Equal(openingBalance, financialPeriod.OpeningBalance);
    }

    [Fact]
    public void PlannedBalance_ShouldIncludeOpeningBalance()
    {
        var financialPeriod = FinancialPeriod.Create(
            Period.Create(2026, 9), TransferredBalance.Create(Money.Create(125_000m, Currency.Clp)));

        Assert.Equal(125_000m, financialPeriod.PlannedBalance.Amount);
    }

    [Fact]
    public void ActualBalance_ShouldAllowNegativeOpeningBalance()
    {
        var financialPeriod = FinancialPeriod.Create(Period.Create(2026, 9), TransferredBalance.Create(Money.Create(-50_000m, Currency.Clp)));

        Assert.Equal(-50_000m, financialPeriod.ActualBalance.Amount);
    }
}