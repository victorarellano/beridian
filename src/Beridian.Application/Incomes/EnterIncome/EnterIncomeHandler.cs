using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;

namespace Beridian.Application.Incomes.EnterIncome;

public sealed class EnterIncomeHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public EnterIncomeHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnterIncomeResult> HandleAsync(EnterIncomeCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);;
        }

        financialPeriod.EnterIncome(command.IncomeId, Money.Create(command.ActualAmount, Currency.Clp));

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new EnterIncomeResult(command.IncomeId);
    }    
}