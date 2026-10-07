using Beridian.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Beridian.Api.Tests.Integration;

public sealed class BeridianWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:16")
            .WithDatabase("beridian_api_tests")
            .WithUsername("beridian")
            .WithPassword("beridian_api_tests_password")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = _container.GetConnectionString()
            };

            configuration.AddInMemoryCollection(settings);
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BeridianDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    public async Task<bool> FinancialPeriodExistsAsync(int year, int month)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BeridianDbContext>();

        return await dbContext.FinancialPeriods
            .AnyAsync(financialPeriod => financialPeriod.Period.Year == year && financialPeriod.Period.Month == month);
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BeridianDbContext>();

        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE financial_periods CASCADE;");
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _container.DisposeAsync();
    }
}