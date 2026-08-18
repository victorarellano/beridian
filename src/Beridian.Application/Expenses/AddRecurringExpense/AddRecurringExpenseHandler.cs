using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;

namespace Beridian.Application.Expenses.AddRecurringExpense;

public sealed class AddRecurringExpenseHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddRecurringExpenseHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddRecurringExpenseResult> HandleAsync(AddRecurringExpenseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        var expense = RecurringExpense.Create(command.Name, Money.Create(command.PlannedAmount, Currency.Clp));

        financialPeriod.AddExpense(expense);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddRecurringExpenseResult(expense.Id);
    }
}