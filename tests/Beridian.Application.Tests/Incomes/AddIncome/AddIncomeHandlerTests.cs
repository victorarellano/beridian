using Beridian.Application.Incomes.AddIncome;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;

namespace Beridian.Application.Tests.Incomes.AddIncome;

public sealed class AddIncomeHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddIncomeAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);
        
        var handler = new AddIncomeHandler(repository);
        var command = new AddIncomeCommand(financialPeriod.Id, "Sueldo Agosto", 2_000_000m);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);
        var incomeStored = repository.UpdatedFinancialPeriod.Incomes.Single(x => x.Id == result.IncomeId);
        
        Assert.Equal(result.IncomeId,incomeStored .Id);
        Assert.Equal(2_000_000m, incomeStored.PlannedAmount.Amount);
        Assert.Equal("Sueldo Agosto", incomeStored.Name);
        Assert.Equal(IncomeStatus.Created, incomeStored.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidExceptionOperation()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddIncomeHandler(repository);
        var command = new AddIncomeCommand(financialPeriod.Id, "Sueldo Agosto", 2_000_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodDoesNotExist_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddIncomeHandler(repository);
        var command = new AddIncomeCommand(Guid.NewGuid(), "Sueldo Agosto", 2_000_000m);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);        
    }

}