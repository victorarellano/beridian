using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Application.Expenses.RemoveExpenseDetail;

public sealed class RemoveExpenseDetailHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public RemoveExpenseDetailHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<RemoveExpenseDetailResult> HandleAsync(RemoveExpenseDetailCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        financialPeriod.RemoveExpenseDetail(command.ExpenseId, command.ExpenseDetailId);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new RemoveExpenseDetailResult(command.ExpenseDetailId);
    }
}