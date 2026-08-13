using Beridian.Application.Abstractions.Events;
using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.Services;

namespace Beridian.Application.FinancialPeriods.GenerateNextFinancialPeriod;

public sealed class GenerateNextFinancialPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly FinancialPeriodGenerator _generator;

    public GenerateNextFinancialPeriodHandler(
        IFinancialPeriodRepository repository, 
        FinancialPeriodGenerator generator,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _repository = repository;
        _generator = generator;
        _domainEventDispatcher = domainEventDispatcher;

    }

    public async Task<GenerateNextFinancialPeriodResult> HandleAsync(GenerateNextFinancialPeriodCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var currentPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (currentPeriod is null)
        {
            throw new InvalidOperationException("Financial period was not found.");
        }

        var nextPeriod = _generator.Generate(currentPeriod);

        await _repository.AddAsync(nextPeriod, cancellationToken);

        await _domainEventDispatcher.DispatchAsync(nextPeriod.DomainEvents, cancellationToken);

        nextPeriod.ClearDomainEvents();

        return new GenerateNextFinancialPeriodResult(nextPeriod.Id, nextPeriod.Period.Year, nextPeriod.Period.Month);
    }
}