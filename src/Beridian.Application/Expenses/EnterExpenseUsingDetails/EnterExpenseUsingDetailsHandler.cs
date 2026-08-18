using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Application.Expenses.EnterExpenseUsingDetails;

public sealed class EnterExpenseUsingDetailsHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public EnterExpenseUsingDetailsHandler(
        IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnterExpenseUsingDetailsResult> HandleAsync(
        EnterExpenseUsingDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);;
        }

        financialPeriod.EnterExpense(command.ExpenseId);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new EnterExpenseUsingDetailsResult(command.ExpenseId);
    }
}