using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Tests.TestDoubles;

internal sealed class FakeFinancialPeriodRepository
    : IFinancialPeriodRepository
{
    private readonly Dictionary<Guid, FinancialPeriod> _periods = [];
    public FinancialPeriod? AddedFinancialPeriod { get; private set; }
    public FinancialPeriod? UpdatedFinancialPeriod { get; private set; }

    public void Seed(FinancialPeriod financialPeriod)
    {
        _periods[financialPeriod.Id] = financialPeriod;
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
}