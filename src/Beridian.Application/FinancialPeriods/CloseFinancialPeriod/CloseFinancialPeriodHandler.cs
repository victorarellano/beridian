using Beridian.Application.Abstractions.Events;
using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Application.FinancialPeriods.CloseFinancialPeriod;

public sealed class CloseFinancialPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CloseFinancialPeriodHandler(
            IFinancialPeriodRepository repository,
            IDomainEventDispatcher domainEventDispatcher)
    {
        _repository = repository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<CloseFinancialPeriodResult> HandleAsync(CloseFinancialPeriodCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken)??
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);

        financialPeriod.Close();

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        await _domainEventDispatcher.DispatchAsync(financialPeriod.DomainEvents, cancellationToken);

        financialPeriod.ClearDomainEvents();

        return new CloseFinancialPeriodResult(financialPeriod.Id);
    }
}