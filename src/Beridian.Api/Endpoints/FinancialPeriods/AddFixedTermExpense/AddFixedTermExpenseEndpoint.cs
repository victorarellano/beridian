using Beridian.Api.Versioning;
using Beridian.Application.Expenses.AddFixedTermExpense;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddFixedTermExpense;

public static class AddFixedTermExpenseEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/fixed-term", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddFixedTermExpenseV1")
            .Produces<AddFixedTermExpenseResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddFixedTermExpenseResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        AddFixedTermExpenseRequest request,
        AddFixedTermExpenseHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = AddFixedTermExpenseRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddFixedTermExpenseCommand(
                financialPeriodId,
                request.Name,
                request.PlannedAmount,
                request.CurrentInstallment,
                request.TotalInstallments);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}