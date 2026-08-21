using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.GenerateNextFinancialPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.GenerateNextFinancialPeriod;

public static class GenerateNextFinancialPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/next", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("GenerateNextFinancialPeriodV1")
            .Produces<GenerateNextFinancialPeriodResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Created<GenerateNextFinancialPeriodResult>> HandleAsync(
        Guid financialPeriodId,
        GenerateNextFinancialPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GenerateNextFinancialPeriodCommand(financialPeriodId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{result.FinancialPeriodId}", result);
    }
}