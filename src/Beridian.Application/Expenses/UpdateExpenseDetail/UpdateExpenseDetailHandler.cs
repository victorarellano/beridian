using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;

namespace Beridian.Application.Expenses.UpdateExpenseDetail;

public sealed class UpdateExpenseDetailHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public UpdateExpenseDetailHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateExpenseDetailResult> HandleAsync(UpdateExpenseDetailCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        var plannedAmount = command.PlannedAmount.HasValue? Money.Create(command.PlannedAmount.Value, Currency.Clp) : null;

        financialPeriod.UpdateExpenseDetail(
            command.ExpenseId, 
            command.ExpenseDetailId, 
            command.Description, 
            Money.Create(command.ActualAmount, Currency.Clp), 
            command.TransactionDate, plannedAmount);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new UpdateExpenseDetailResult(command.ExpenseDetailId);
    }
}