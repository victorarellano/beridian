using Beridian.Api.Versioning;
using Beridian.Application.Expenses.AddDiscretionaryExpense;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddDiscretionaryExpense;

public static class AddDiscretionaryExpenseEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/discretionary", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddDiscretionaryExpenseV1")
            .Produces<AddDiscretionaryExpenseResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddDiscretionaryExpenseResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        AddDiscretionaryExpenseRequest request,
        AddDiscretionaryExpenseHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = AddDiscretionaryExpenseRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddDiscretionaryExpenseCommand(
                financialPeriodId,
                request.Name);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}