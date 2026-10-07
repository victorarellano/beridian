using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.FinancialPeriods.GetFinancialPeriodByPeriod;

public sealed class GetFinancialPeriodByPeriodHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public GetFinancialPeriodByPeriodHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetFinancialPeriodResult> HandleAsync(GetFinancialPeriodByPeriodQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var period = Period.Create(query.Year, query.Month);
        var financialPeriod = await _repository.GetByPeriodAsync(period, cancellationToken)
            ?? throw new FinancialPeriodByPeriodNotFoundException(query.Year, query.Month);

        return GetFinancialPeriodResultMapper.Map(financialPeriod);
    }
}