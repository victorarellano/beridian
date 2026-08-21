using Beridian.Domain.Investments.Exceptions;

namespace Beridian.Api.ExceptionHandling;

internal sealed class InvestmentAlreadyConfirmedExceptionHandler
    : ApiExceptionHandler<InvestmentAlreadyConfirmedException>
{
    protected override ApiProblem CreateProblem(InvestmentAlreadyConfirmedException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Investment already confirmed",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["investmentId"] = exception.InvestmentId
            });
    }



}