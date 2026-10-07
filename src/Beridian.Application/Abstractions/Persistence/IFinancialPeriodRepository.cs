using Beridian.Domain.FinancialPeriods;

namespace Beridian.Application.Abstractions.Persistence;
public interface IFinancialPeriodRepository
{

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<FinancialPeriod?> GetByPeriodAsync(Period period, CancellationToken cancellationToken = default);
    Task<FinancialPeriod?> GetLatestAsync(CancellationToken cancellationToken = default);
    Task<FinancialPeriod?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPeriodAsync(Period period, CancellationToken cancellationToken = default);
    Task UpdateAsync(FinancialPeriod financialPeriod, CancellationToken cancellationToken = default);
}