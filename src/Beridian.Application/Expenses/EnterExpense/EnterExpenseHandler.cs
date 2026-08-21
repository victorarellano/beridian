using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;

namespace Beridian.Application.Expenses.EnterExpense;

public sealed class EnterExpenseHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public EnterExpenseHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnterExpenseResult> HandleAsync(
        EnterExpenseCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        financialPeriod.EnterExpense(command.ExpenseId, Money.Create(command.ActualAmount, Currency.Clp));

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new EnterExpenseResult(command.ExpenseId);
    }
}