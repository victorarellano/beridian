using Asp.Versioning;

namespace Beridian.Api.Versioning;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddApiVersioningConfiguration(
        this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersions.V1;
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}