using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.CreateFinancialPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.CreateFinancialPeriod;
public static class CreateFinancialPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("CreateFinancialPeriodV1")
            .Produces<CreateFinancialPeriodResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
    private static async Task<Results<Created<CreateFinancialPeriodResult>, ValidationProblem>> HandleAsync(
        CreateFinancialPeriodRequest request,
        CreateFinancialPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = CreateFinancialPeriodRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }        
    
        var command = new CreateFinancialPeriodCommand(request.Year, request.Month);
        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{result.FinancialPeriodId}", result);
    }
}