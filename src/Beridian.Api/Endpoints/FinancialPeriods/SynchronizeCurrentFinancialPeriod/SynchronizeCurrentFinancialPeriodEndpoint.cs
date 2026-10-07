using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.SynchronizeCurrentFinancialPeriod;

public static class SynchronizeCurrentFinancialPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/synchronize", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("SynchronizeCurrentFinancialPeriodV1")
            .Produces<SynchronizeCurrentFinancialPeriodResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Ok<SynchronizeCurrentFinancialPeriodResult>,ValidationProblem>> HandleAsync(
        SynchronizeCurrentFinancialPeriodRequest request, SynchronizeCurrentFinancialPeriodHandler handler, CancellationToken cancellationToken)
    {
        var validationErrors = SynchronizeCurrentFinancialPeriodRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        } 

        var command = new SynchronizeCurrentFinancialPeriodCommand(request.Year, request.Month);
        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}