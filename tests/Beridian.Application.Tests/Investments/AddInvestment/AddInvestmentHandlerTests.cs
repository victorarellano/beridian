using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.Investments.AddInvestment;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Exceptions;
using Beridian.Domain.Investments;

namespace Beridian.Application.Tests.Investments.AddInvestment;

public sealed class AddInvestmentHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddInvestmentAndPersistPeriod()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new AddInvestmentHandler(repository);

        var command = new AddInvestmentCommand(
            financialPeriod.Id,
            "Monthly Investment",
            300_000m);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var storedInvestment =
            repository.UpdatedFinancialPeriod.Investments
                .Single(x => x.Id == result.InvestmentId);

        Assert.Equal(result.InvestmentId, storedInvestment.Id);
        Assert.Equal("Monthly Investment", storedInvestment.Name);
        Assert.Equal(300_000m, storedInvestment.PlannedAmount.Amount);
        Assert.Equal(0m, storedInvestment.ActualAmount.Amount);
        Assert.Equal(InvestmentStatus.Created, storedInvestment.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddInvestmentHandler(repository);

        var command = new AddInvestmentCommand(
            Guid.NewGuid(),
            "Monthly Investment",
            300_000m);

        // Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodNotFoundException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddInvestmentHandler(repository);

        var command = new AddInvestmentCommand(
            financialPeriod.Id,
            "Monthly Investment",
            300_000m);

        // Act & Assert
        await Assert.ThrowsAsync<FinancialPeriodClosedException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
        Assert.Empty(financialPeriod.Investments);
    }
}