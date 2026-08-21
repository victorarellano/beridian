using Beridian.Application.Expenses.EnterExpense;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.EnterExpense;

public static class EnterExpenseEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/{expenseId:guid}/entry", HandleAsync)
            .MapToApiVersion(1.0)
            .WithName("EnterExpenseV1")
            .Produces<EnterExpenseResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Ok<EnterExpenseResult>, ValidationProblem>> HandleAsync(
            Guid financialPeriodId,
            Guid expenseId,
            EnterExpenseRequest request,
            EnterExpenseHandler handler,
            CancellationToken cancellationToken)
    {
        
        var validationErrors = EnterExpenseRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new EnterExpenseCommand(
            financialPeriodId,
            expenseId,
            request.ActualAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}