using Asp.Versioning;
using Beridian.Application.Expenses.EnterExpenseUsingDetails;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.EnterExpenseUsingDetails;

public static class EnterExpenseUsingDetailsEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/expenses/{expenseId:guid}/entry-from-details", HandleAsync)
            .MapToApiVersion(1.0)
            .WithName("EnterExpenseUsingDetailsV1")
            .Produces<EnterExpenseUsingDetailsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Ok<EnterExpenseUsingDetailsResult>> HandleAsync(
            Guid financialPeriodId,
            Guid expenseId,
            EnterExpenseUsingDetailsHandler handler,
            CancellationToken cancellationToken)
    {
        var command = new EnterExpenseUsingDetailsCommand(financialPeriodId, expenseId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}