using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.FinancialPeriods.GetFinancialPeriod;

public sealed class GetFinancialPeriodHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodExists_ShouldReturnFinancialPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 9));

        repository.Seed(financialPeriod);

        var handler = new GetFinancialPeriodHandler(repository);

        var query = new GetFinancialPeriodQuery(financialPeriod.Id);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.Equal(financialPeriod.Id, result.FinancialPeriodId);

        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.Equal("Open", result.Status);

        Assert.Equal(0m, result.OpeningBalance.Amount);
        Assert.Equal(0m, result.PlannedBalance.Amount);
        Assert.Equal(0m, result.ActualBalance.Amount);

        Assert.Empty(result.Expenses);
        Assert.Empty(result.Incomes);
        Assert.Empty(result.Investments);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowFinancialPeriodNotFoundException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new GetFinancialPeriodHandler(repository);

        var financialPeriodId = Guid.NewGuid();

        var query = new GetFinancialPeriodQuery(financialPeriodId);

        // Act
        var action = async () => await handler.HandleAsync(query);

        // Assert
        var exception = await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(action);

        Assert.Equal(financialPeriodId, exception.FinancialPeriodId);
    }
}