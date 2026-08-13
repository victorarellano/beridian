using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.FinancialPeriods.CreateFinancialPeriod;

public sealed class CreateFinancialPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public CreateFinancialPeriodHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateFinancialPeriodResult> HandleAsync(CreateFinancialPeriodCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var period = Period.Create(command.Year, command.Month);

        var financialPeriod = FinancialPeriod.CreateInitial(period);

        await _repository.AddAsync(financialPeriod, cancellationToken);

        return new CreateFinancialPeriodResult(financialPeriod.Id, financialPeriod.Period.Year, financialPeriod.Period.Month);
    }
}