using Beridian.Api.Versioning;
using Beridian.Application.Incomes.AddIncome;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddIncome;

public static class AddIncomeEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/incomes", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddIncomeV1")
            .Produces<AddIncomeResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddIncomeResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        AddIncomeRequest request,
        AddIncomeHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = AddIncomeRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddIncomeCommand(financialPeriodId, request.Name, request.PlannedAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}