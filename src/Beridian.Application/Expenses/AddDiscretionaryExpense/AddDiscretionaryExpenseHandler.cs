using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;

namespace Beridian.Application.Expenses.AddDiscretionaryExpense;

public sealed class AddDiscretionaryExpenseHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddDiscretionaryExpenseHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddDiscretionaryExpenseResult> HandleAsync(AddDiscretionaryExpenseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new InvalidOperationException("Financial period was not found.");
        }

        var expense = DiscretionaryExpense.Create(command.Name, Currency.Clp);

        financialPeriod.AddExpense(expense);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddDiscretionaryExpenseResult(expense.Id);
    }
}