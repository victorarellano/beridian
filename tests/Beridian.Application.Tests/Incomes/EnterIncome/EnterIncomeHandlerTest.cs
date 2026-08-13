using Beridian.Application.Expenses.EnterExpense;
using Beridian.Application.Incomes.EnterIncome;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;

namespace Beridian.Application.Tests.Incomes.EnterIncome;

public sealed class EnterIncomeHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldEnterIncomeAndPersistPeriod()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        var income = Income.Create("Sueldo Agosto", Money.Create(2_000_000m, Currency.Clp));
        financialPeriod.AddIncome(income);

        repository.Seed(financialPeriod);

        var handler = new EnterIncomeHandler(repository);
        var command =  new EnterIncomeCommand(financialPeriod.Id, income.Id, 2_100_000m);

        //Act
        var result = await handler.HandleAsync(command);
        
        //Assert
        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var incomeStored = repository.UpdatedFinancialPeriod.Incomes.Single(x => x.Id == result.IncomeId);

        Assert.Equal(result.IncomeId, incomeStored.Id);
        Assert.Equal("Sueldo Agosto", incomeStored.Name);
        Assert.Equal(2_000_000m, incomeStored.PlannedAmount.Amount);
        Assert.Equal(2_100_000m, incomeStored.ActualAmount.Amount);
        Assert.Equal(IncomeStatus.Entered, incomeStored.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenIncomeDoesNotBelongToPeriod_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new EnterIncomeHandler(repository);

        var command = new EnterIncomeCommand(
            financialPeriod.Id,
            Guid.NewGuid(),
            2_100_000m);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        
        var income = Income.Create("Sueldo Agosto", Money.Create(2_000_000m, Currency.Clp));
        
        financialPeriod.AddIncome(income);

        financialPeriod.EnterIncome(income.Id, Money.Create(2_050_000m, Currency.Clp));

        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new EnterIncomeHandler(repository);
        var command =  new EnterIncomeCommand(financialPeriod.Id, income.Id, 2_100_000m);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        
        Assert.Null(repository.UpdatedFinancialPeriod);

        var storedIncome = financialPeriod.Incomes.Single(x => x.Id == income.Id);

        Assert.Equal("Sueldo Agosto", storedIncome.Name);
        Assert.Equal(2_000_000m, storedIncome.PlannedAmount.Amount);
        Assert.Equal(2_050_000m, storedIncome.ActualAmount.Amount);
        Assert.Equal(IncomeStatus.Entered, storedIncome.Status);        
    }
}