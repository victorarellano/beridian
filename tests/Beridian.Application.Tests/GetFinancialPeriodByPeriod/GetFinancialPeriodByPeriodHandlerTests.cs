using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.FinancialPeriods.GetFinancialPeriodByPeriod;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.FinancialPeriods.GetFinancialPeriodByPeriod;

public sealed class GetFinancialPeriodByPeriodHandlerTests
{

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodExists_ShouldReturnFinancialPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 10));
        repository.Seed(financialPeriod);

        var handler = new GetFinancialPeriodByPeriodHandler(repository);
        var query = new GetFinancialPeriodByPeriodQuery(2026, 10);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.Equal(financialPeriod.Id, result.FinancialPeriodId);
        Assert.Equal(2026, result.Year);
        Assert.Equal(10, result.Month);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowFinancialPeriodByPeriodNotFoundException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new GetFinancialPeriodByPeriodHandler(repository);
        var query = new GetFinancialPeriodByPeriodQuery(2026, 10);

        // Act
        var exception = await Assert.ThrowsAsync<FinancialPeriodByPeriodNotFoundException>(() => handler.HandleAsync(query));

        // Assert
        Assert.Equal(2026, exception.Year);
        Assert.Equal(10, exception.Month);
    }    
}
