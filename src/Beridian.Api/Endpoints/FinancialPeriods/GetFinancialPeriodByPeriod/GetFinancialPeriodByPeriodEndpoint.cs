
using Beridian.Api.Versioning;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Beridian.Application.FinancialPeriods.GetFinancialPeriodByPeriod;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.GetFinancialPeriodByPeriod;
public static class GetFinancialPeriodByPeriodEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/by-period", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("GetFinancialPeriodByPeriodV1")
            .Produces<GetFinancialPeriodResult>(
                StatusCodes.Status200OK)
            .ProducesValidationProblem()                
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }
    private static async Task<Results<Ok<GetFinancialPeriodResult>, ValidationProblem>>  HandleAsync(
        int year,
        int month,
        GetFinancialPeriodByPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = GetFinancialPeriodByPeriodRequestValidator.Validate(year, month);
        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }  

        var query = new GetFinancialPeriodByPeriodQuery(year, month);
        var result = await handler.HandleAsync(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}