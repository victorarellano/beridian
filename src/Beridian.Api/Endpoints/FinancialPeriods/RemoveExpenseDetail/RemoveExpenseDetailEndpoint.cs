using Beridian.Api.Versioning;
using Beridian.Application.Expenses.RemoveExpenseDetail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.RemoveExpenseDetail;

public static class RemoveExpenseDetailEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapDelete(
                "/{financialPeriodId:guid}/expenses/{expenseId:guid}/details/{expenseDetailId:guid}", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("RemoveExpenseDetailV1")
            .Produces<RemoveExpenseDetailResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Ok<RemoveExpenseDetailResult>> HandleAsync(
        Guid financialPeriodId, Guid expenseId, Guid expenseDetailId, RemoveExpenseDetailHandler handler, CancellationToken cancellationToken)
    {
        var command = new RemoveExpenseDetailCommand(financialPeriodId, expenseId, expenseDetailId);
        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}