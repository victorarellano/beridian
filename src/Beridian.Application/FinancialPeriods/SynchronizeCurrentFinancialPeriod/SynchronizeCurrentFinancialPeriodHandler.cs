using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.Exceptions;
using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;

public sealed class SynchronizeCurrentFinancialPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public SynchronizeCurrentFinancialPeriodHandler(
        IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<SynchronizeCurrentFinancialPeriodResult> HandleAsync(SynchronizeCurrentFinancialPeriodCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var currentPeriod = Period.Create(command.Year, command.Month);

        var existingFinancialPeriod = await _repository.GetByPeriodAsync(currentPeriod, cancellationToken);

        if (existingFinancialPeriod is not null)
        {
            return new SynchronizeCurrentFinancialPeriodResult(existingFinancialPeriod.Id, existingFinancialPeriod.Period.Year, existingFinancialPeriod.Period.Month, false);
        }

        var hasFinancialHistory = await _repository.AnyAsync(cancellationToken);

        if (!hasFinancialHistory)
        {
            var financialPeriod = FinancialPeriod.CreateInitial(currentPeriod);

            await _repository.AddAsync(financialPeriod, cancellationToken);

            return new SynchronizeCurrentFinancialPeriodResult(financialPeriod.Id, financialPeriod.Period.Year, financialPeriod.Period.Month, true);
        }

        // History exists, but the current period does not.
        // Automatic catch-up belongs to BC-005 and is deferred.
        throw new CurrentFinancialPeriodNotAvailableException(command.Year, command.Month);
    }
}