using Microsoft.Extensions.DependencyInjection;

namespace Beridian.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services;
    }
}