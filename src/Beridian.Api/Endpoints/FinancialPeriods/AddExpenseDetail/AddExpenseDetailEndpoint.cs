using Beridian.Api.Versioning;
using Beridian.Application.Expenses.AddExpenseDetail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddExpenseDetail;

public static class AddExpenseDetailEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/{expenseId:guid}/details", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddExpenseDetailV1")
            .Produces<AddExpenseDetailResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddExpenseDetailResult>, ValidationProblem>> HandleAsync(
        Guid financialPeriodId,
        Guid expenseId,
        AddExpenseDetailRequest request,
        AddExpenseDetailHandler handler,
        CancellationToken cancellationToken)
    {
        var validationErrors = AddExpenseDetailRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddExpenseDetailCommand(
                financialPeriodId,
                expenseId,
                request.Description,
                request.ActualAmount,
                request.TransactionDate,
                request.PlannedAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}