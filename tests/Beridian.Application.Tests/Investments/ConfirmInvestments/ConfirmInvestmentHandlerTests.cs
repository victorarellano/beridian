using Beridian.Application.Investments.ConfirmInvestment;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Investments;

namespace Beridian.Application.Tests.Investments.ConfirmInvestment;

public sealed class ConfirmInvestmentHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldConfirmInvestmentAndPersistPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var investment = Investment.Create("Monthly Investment", Money.Create(300_000m, Currency.Clp));

        financialPeriod.AddInvestment(investment);

        repository.Seed(financialPeriod);

        var handler = new ConfirmInvestmentHandler(repository);
        var command = new ConfirmInvestmentCommand(financialPeriod.Id, investment.Id, 280_000m);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var storedInvestment = repository.UpdatedFinancialPeriod.Investments.Single(x => x.Id == investment.Id);

        Assert.Equal(result.InvestmentId, storedInvestment.Id);
        Assert.Equal(280_000m, storedInvestment.ActualAmount.Amount);
        Assert.Equal(InvestmentStatus.Confirmed, storedInvestment.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new ConfirmInvestmentHandler(repository);

        var command = new ConfirmInvestmentCommand(Guid.NewGuid(), Guid.NewGuid(), 280_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenInvestmentDoesNotBelongToPeriod_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new ConfirmInvestmentHandler(repository);

        var command = new ConfirmInvestmentCommand(financialPeriod.Id, Guid.NewGuid(), 280_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var investment = Investment.Create("Monthly Investment", Money.Create(300_000m, Currency.Clp));

        financialPeriod.AddInvestment(investment);

        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new ConfirmInvestmentHandler(repository);

        var command = new ConfirmInvestmentCommand(financialPeriod.Id, investment.Id, 280_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);

        var storedInvestment = financialPeriod.Investments.Single(x => x.Id == investment.Id);

        Assert.Equal(0m, storedInvestment.ActualAmount.Amount);
        Assert.Equal(InvestmentStatus.Created, storedInvestment.Status);
    }
}