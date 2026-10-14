using Beridian.Api.Endpoints.FinancialPeriods.UpdateExpenseDetail;
using Beridian.Api.Versioning;
using Beridian.Application.Expenses.UpdateExpenseDetail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods;

public static class  UpdateExpenseDetailEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/{expenseId:guid}/details/{expenseDetailId:guid}", HandleAsync)
            .MapToApiVersion(1.0)
            .WithName("UpdateExpenseDetailV1")
            .Produces<UpdateExpenseDetailResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Ok<UpdateExpenseDetailResult>, ValidationProblem>> HandleAsync(
            Guid financialPeriodId,
            Guid expenseId,
            Guid expenseDetailId,
            UpdateExpenseDetailRequest request,
            UpdateExpenseDetailHandler handler,
            CancellationToken cancellationToken)
    {
        var validationErrors = UpdateExpenseDetailRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new UpdateExpenseDetailCommand(
            financialPeriodId, 
            expenseId, 
            expenseDetailId,
            request.Description,
            request.ActualAmount,
            request.TransactionDate,
            request.PlannedAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }    
}