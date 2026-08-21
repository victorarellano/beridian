using Beridian.Api.Versioning;
using Beridian.Application.Investments.ConfirmInvestment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Beridian.Api.Endpoints.FinancialPeriods.ConfirmInvestment;

public static class ConfirmInvestmentEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{financialPeriodId:guid}/investments/{investmentId:guid}/confirmation", HandleAsync)
            .MapToApiVersion(ApiVersions.V1)
            .WithName("ConfirmInvestmentV1")
            .Produces<ConfirmInvestmentResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<Ok<ConfirmInvestmentResult>, ValidationProblem>> HandleAsync(
            Guid financialPeriodId,
            Guid investmentId,
            ConfirmInvestmentRequest request,
            ConfirmInvestmentHandler handler,
            CancellationToken cancellationToken)
    {
        var validationErrors = ConfirmInvestmentRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var command = new ConfirmInvestmentCommand(
            financialPeriodId,
            investmentId,
            request.ActualAmount);

        var result = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}