using Beridian.Api.Versioning;
using Beridian.Application.Expenses.AddRecurringExpense;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddRecurringExpense;

public static class AddRecurringExpenseEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/recurring", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddRecurringExpenseV1")
            .Produces<AddRecurringExpenseResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddRecurringExpenseResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        AddRecurringExpenseRequest request,
        AddRecurringExpenseHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = AddRecurringExpenseRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddRecurringExpenseCommand(
                financialPeriodId,
                request.Name,
                request.PlannedAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}