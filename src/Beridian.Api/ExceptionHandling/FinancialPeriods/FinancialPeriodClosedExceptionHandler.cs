using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class FinancialPeriodClosedExceptionHandler : ApiExceptionHandler<FinancialPeriodClosedException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodClosedException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Financial period is closed",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId
            });
    }
}