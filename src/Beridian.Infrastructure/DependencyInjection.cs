using Beridian.Application.Abstractions.Events;
using Beridian.Application.Abstractions.Persistence;
using Beridian.Infrastructure.Events;
using Beridian.Infrastructure.Persistence;
using Beridian.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beridian.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException(
                "The database connection string was not configured.");

        services.AddDbContext<BeridianDbContext>(
            options => options.UseNpgsql(connectionString));

        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();            
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}