using Beridian.Domain.Expenses;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;
using Beridian.Infrastructure.Persistence.Repositories;
using Beridian.Infrastructure.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace Beridian.Infrastructure.Tests.Persistence.Configurations;

[Collection(PostgreSqlCollection.Name)]
public sealed class FinancialPeriodPersistenceTests
{
    private readonly PostgreSqlFixture _fixture;

    public FinancialPeriodPersistenceTests(
        PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Delete_WhenFinancialPeriodExists_ShouldCascadeToRelatedEntities()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var financialPeriod = FinancialPeriodTestData.CreateComplete();

        await using (var seedDbContext = _fixture.CreateDbContext())
        {
            var repository = new FinancialPeriodRepository(seedDbContext);

            await repository.AddAsync(financialPeriod);
        }

        // Act
        await using (var deleteDbContext = _fixture.CreateDbContext())
        {
            await deleteDbContext.Database
                .ExecuteSqlInterpolatedAsync(
                    $"""
                    DELETE FROM financial_periods
                    WHERE id = {financialPeriod.Id}
                    """);
        }

        // Assert
        await using var verificationDbContext = _fixture.CreateDbContext();

        Assert.Empty(
            await verificationDbContext
                .Set<Expense>()
                .AsNoTracking()
                .ToListAsync());

        Assert.Empty(
            await verificationDbContext
                .Set<ExpenseDetail>()
                .AsNoTracking()
                .ToListAsync());

        Assert.Empty(
            await verificationDbContext
                .Set<Income>()
                .AsNoTracking()
                .ToListAsync());

        Assert.Empty(
            await verificationDbContext
                .Set<Investment>()
                .AsNoTracking()
                .ToListAsync());
    }
}