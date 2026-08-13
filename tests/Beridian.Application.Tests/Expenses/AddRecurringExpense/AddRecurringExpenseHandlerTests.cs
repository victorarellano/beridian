using Beridian.Application.Expenses.AddRecurringExpense;
using Beridian.Application.Tests.TestDoubles;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.Expenses.AddRecurringExpense;

public sealed class AddRecurringExpenseHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldAddRecurringExpenseAndPersistPeriod()
    {
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        repository.Seed(financialPeriod);

        var handler = new AddRecurringExpenseHandler(repository);

        var command = new AddRecurringExpenseCommand(financialPeriod.Id, "Electricity", 50_000m);

        var result = await handler.HandleAsync(command);

        Assert.NotNull(repository.UpdatedFinancialPeriod);

        var expense = Assert.IsType<RecurringExpense>(Assert.Single(repository.UpdatedFinancialPeriod.Expenses));

        Assert.Equal("Electricity", expense.Name);
        Assert.Equal(50_000m, expense.PlannedAmount.Amount);
        Assert.Equal(expense.Id, result.ExpenseId);
    }

    [Fact]
    public async Task HandleAsync_WhenPeriodDoesNotExist_ShouldThrowInvalidOperationExceptionByRecurringExpense()
    {
        var repository = new FakeFinancialPeriodRepository();

        var handler = new AddRecurringExpenseHandler(repository);

        var command = new AddRecurringExpenseCommand(Guid.NewGuid(), "Electricity", 50_000m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Null(repository.UpdatedFinancialPeriod);
    }

    [Fact]
    public async Task HandleAsync_WhenFinancialPeriodIsClosed_ShouldThrowInvalidOperationExceptionForRecurringExpense()
    {
        var repository = new FakeFinancialPeriodRepository();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));
        financialPeriod.Close();

        repository.Seed(financialPeriod);

        var handler = new AddRecurringExpenseHandler(repository);
        var command = new AddRecurringExpenseCommand(Guid.NewGuid(), "Electricity", 50_000m);


        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        Assert.Empty(financialPeriod.Expenses);
        Assert.Null(repository.UpdatedFinancialPeriod);
    }
}