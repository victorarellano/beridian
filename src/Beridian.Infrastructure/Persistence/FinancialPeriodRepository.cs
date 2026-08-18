using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.FinancialPeriods;
using Microsoft.EntityFrameworkCore;

namespace Beridian.Infrastructure.Persistence.Repositories;

public sealed class FinancialPeriodRepository : IFinancialPeriodRepository
{
    private readonly BeridianDbContext _dbContext;

    public FinancialPeriodRepository(BeridianDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FinancialPeriod?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.FinancialPeriods
            .Include(financialPeriod => financialPeriod.Expenses)
                .ThenInclude(expense => expense.Details)
            .Include(financialPeriod => financialPeriod.Incomes)
            .Include(financialPeriod => financialPeriod.Investments)
            .AsSplitQuery()
            .SingleOrDefaultAsync(
                financialPeriod => financialPeriod.Id == id,
                cancellationToken);
    }

    public async Task<bool> ExistsByPeriodAsync(Period period, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(period);

        return await _dbContext.FinancialPeriods
            .AnyAsync(
                financialPeriod =>
                    financialPeriod.Period.Year == period.Year &&
                    financialPeriod.Period.Month == period.Month,
                cancellationToken);
    }
    public async Task AddAsync(
        FinancialPeriod financialPeriod,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(financialPeriod);

        await _dbContext.FinancialPeriods.AddAsync(financialPeriod, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

    }

    public async Task UpdateAsync(
        FinancialPeriod financialPeriod,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(financialPeriod);

        if (_dbContext.Entry(financialPeriod).State == EntityState.Detached)
        {
            throw new InvalidOperationException(
                "The financial period must be loaded by this repository before it can be updated.");
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}