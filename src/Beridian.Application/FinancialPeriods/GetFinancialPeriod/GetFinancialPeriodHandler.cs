using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Application.FinancialPeriods.GetFinancialPeriod;

public sealed class GetFinancialPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public GetFinancialPeriodHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetFinancialPeriodResult> HandleAsync(GetFinancialPeriodQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var financialPeriod = await _repository.GetByIdAsync(query.FinancialPeriodId, cancellationToken)
            ?? throw new FinancialPeriodNotFoundException(query.FinancialPeriodId);

        return GetFinancialPeriodResultMapper.Map(financialPeriod);
    }
}