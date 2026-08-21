using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
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
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

    
        var expense = FixedTermExpense.Create(command.Name, Money.Create(command.PlannedAmount, Currency.Clp), command.CurrentInstallment, command.TotalInstallments);

        financialPeriod.AddExpense(expense);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddFixedTermExpenseResult(expense.Id);
    }
}