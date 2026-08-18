using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class FinancialPeriodAlreadyExistsExceptionHandler
    : ApiExceptionHandler<FinancialPeriodAlreadyExistsException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodAlreadyExistsException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Financial period already exists",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["year"] = exception.Year,
                ["month"] = exception.Month
            });
    }
}