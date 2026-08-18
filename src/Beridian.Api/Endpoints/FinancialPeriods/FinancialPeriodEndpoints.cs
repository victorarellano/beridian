using Beridian.Api.Endpoints.FinancialPeriods.CloseFinancialPeriod;
using Beridian.Api.Endpoints.FinancialPeriods.CreateFinancialPeriod;
using Beridian.Api.Endpoints.FinancialPeriods.GetFinancialPeriod;
using Beridian.Api.Versioning;

namespace Beridian.Api.Endpoints.FinancialPeriods;

public static class FinancialPeriodEndpoints
{
    public static IEndpointRouteBuilder MapFinancialPeriodEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(ApiVersions.V1)
            .ReportApiVersions()
            .Build();

        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/financial-periods")
            .WithApiVersionSet(versionSet)
            .WithTags("Financial Periods");

        CreateFinancialPeriodEndpoint.Map(group);
        GetFinancialPeriodEndpoint.Map(group);
        CloseFinancialPeriodEndpoint.Map(group);

        return endpoints;
    }
}