
using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.GetFinancialPeriod;
public static class GetFinancialPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{financialPeriodId:guid}", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("GetFinancialPeriodV1")
            .Produces<GetFinancialPeriodResult>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<Ok<GetFinancialPeriodResult>> HandleAsync(
        Guid financialPeriodId,
        GetFinancialPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetFinancialPeriodQuery(financialPeriodId);
        var result = await handler.HandleAsync(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}