using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;

namespace Beridian.Application.Expenses.AddFixedTermExpense;

public sealed class AddFixedTermExpenseHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddFixedTermExpenseHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddFixedTermExpenseResult> HandleAsync(AddFixedTermExpenseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new InvalidOperationException("Financial period was not found.");
        }

        var expense = FixedTermExpense.Create(command.Name, command.plannedAmmount, command.currentInstallment, command.totalInstallments);

        financialPeriod.AddExpense(expense);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddFixedTermExpenseResult(expense.Id);
    }
}