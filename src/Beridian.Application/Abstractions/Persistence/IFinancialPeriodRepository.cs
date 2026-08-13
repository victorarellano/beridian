using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Abstractions.Persistence;
public interface IFinancialPeriodRepository
{
    Task<FinancialPeriod?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default);

    Task UpdateAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default);
}