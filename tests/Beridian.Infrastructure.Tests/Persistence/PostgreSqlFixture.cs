using Beridian.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Beridian.Infrastructure.Tests.Persistence;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = 
        new PostgreSqlBuilder("postgres:16")
            .WithDatabase("beridian_tests")
            .WithUsername("beridian")
            .WithPassword("beridian_tests_password")
            .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var dbContext = CreateDbContext();

        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public BeridianDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BeridianDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        return new BeridianDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var dbContext = CreateDbContext();

        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE financial_periods CASCADE;");
    }
}