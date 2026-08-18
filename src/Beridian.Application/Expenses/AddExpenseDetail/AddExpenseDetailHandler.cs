using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;
using Beridian.Domain.Expenses;

namespace Beridian.Application.Expenses.AddExpenseDetail;

public sealed class AddExpenseDetailHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddExpenseDetailHandler(
        IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddExpenseDetailResult> HandleAsync(
        AddExpenseDetailCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        var plannedAmount = command.PlannedAmount.HasValue
            ? Money.Create(
                command.PlannedAmount.Value,
                Currency.Clp)
            : null;

        var detail = ExpenseDetail.Create(
            command.Description,
            Money.Create(
                command.ActualAmount,
                Currency.Clp),
            command.TransactionDate,
            plannedAmount);

        financialPeriod.AddExpenseDetail(
            command.ExpenseId,
            detail);

        await _repository.UpdateAsync(
            financialPeriod,
            cancellationToken);

        return new AddExpenseDetailResult(
            detail.Id);
    }
}