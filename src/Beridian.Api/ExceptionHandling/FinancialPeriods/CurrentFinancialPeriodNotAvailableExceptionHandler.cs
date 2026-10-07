using Beridian.Application.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class CurrentFinancialPeriodNotAvailableExceptionHandler
    : ApiExceptionHandler<CurrentFinancialPeriodNotAvailableException>
{
    protected override ApiProblem CreateProblem(CurrentFinancialPeriodNotAvailableException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Current financial period not available",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["year"] = exception.Year,
                ["month"] = exception.Month
            });
    }
}