using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class FinancialPeriodByPeriodNotFoundExceptionHandler
    : ApiExceptionHandler<FinancialPeriodByPeriodNotFoundException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodByPeriodNotFoundException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Financial period not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["year"] = exception.Year,
                ["month"] = exception.Month
            });
    }
}