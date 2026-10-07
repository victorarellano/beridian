using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.TestDoubles;

internal sealed class FakeFinancialPeriodRepository : IFinancialPeriodRepository
{
    private readonly Dictionary<Guid, FinancialPeriod> _periods = [];
    public FinancialPeriod? AddedFinancialPeriod { get; private set; }
    public FinancialPeriod? UpdatedFinancialPeriod { get; private set; }

    public void Seed(FinancialPeriod financialPeriod)
    {
        _periods[financialPeriod.Id] = financialPeriod;
    }

    public Task<bool> ExistsByPeriodAsync(Period period, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(period);

        var exists = _periods.Values.Any(
            financialPeriod =>
                financialPeriod.Period.Year == period.Year &&
                financialPeriod.Period.Month == period.Month);

        return Task.FromResult(exists);
    }    

    public Task AddAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default)
    {
        AddedFinancialPeriod = financialPeriod;
        _periods[financialPeriod.Id] = financialPeriod;

        return Task.CompletedTask;
    }

    public Task<FinancialPeriod?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _periods.TryGetValue(id, out var financialPeriod);

        return Task.FromResult(financialPeriod);
    }

    public Task UpdateAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default)
    {
        _periods[financialPeriod.Id] = financialPeriod;
        UpdatedFinancialPeriod = financialPeriod;

        return Task.CompletedTask;
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_periods.Any());
    }

    public Task<FinancialPeriod?> GetByPeriodAsync(Period period, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(period);

        var financialPeriod = _periods.Values
        .SingleOrDefault(x => x.Period.Year == period.Year && x.Period.Month == period.Month);

        return Task.FromResult(financialPeriod);
    }

    public Task<FinancialPeriod?> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var financialPeriod = _periods.Values
            .OrderByDescending(x => x.Period.Year)
            .ThenByDescending(x => x.Period.Month)
            .FirstOrDefault();

        return Task.FromResult(financialPeriod);
    }
}