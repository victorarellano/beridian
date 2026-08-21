using Beridian.Api.Versioning;
using Beridian.Application.Investments.AddInvestment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.AddInvestment;

public static class AddInvestmentEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/investments", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("AddInvestmentV1")
            .Produces<AddInvestmentResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Created<AddInvestmentResult>, ValidationProblem>> HandleAsync(
            Guid financialPeriodId,
            AddInvestmentRequest request,
            AddInvestmentHandler handler,
            CancellationToken cancellationToken)
    {
        var validationErrors = AddInvestmentRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new AddInvestmentCommand(
            financialPeriodId,
            request.Name,
            request.PlannedAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/api/v1/financial-periods/{financialPeriodId}", result);
    }
}