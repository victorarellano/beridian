using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.CloseFinancialPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.CloseFinancialPeriod;

public static class CloseFinancialPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/close", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("CloseFinancialPeriodV1")
            .Produces<CloseFinancialPeriodResult>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound)
            .ProducesProblem(
                StatusCodes.Status409Conflict);
    }

    private static async Task<Ok<CloseFinancialPeriodResult>> HandleAsync(
        Guid financialPeriodId,
        CloseFinancialPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CloseFinancialPeriodCommand(financialPeriodId);
        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}