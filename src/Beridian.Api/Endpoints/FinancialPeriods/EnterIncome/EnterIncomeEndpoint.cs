using Beridian.Api.Versioning;
using Beridian.Application.Incomes.EnterIncome;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.EnterIncome;

public static class EnterIncomeEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/incomes/{incomeId:guid}/entry", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("EnterIncomeV1")
            .Produces<EnterIncomeResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Ok<EnterIncomeResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        Guid incomeId,
        EnterIncomeRequest request,
        EnterIncomeHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = EnterIncomeRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new EnterIncomeCommand(
                financialPeriodId,
                incomeId,
                request.ActualAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}