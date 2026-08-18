using Beridian.Application.FinancialPeriods.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class FinancialPeriodNotFoundExceptionHandler
    : ApiExceptionHandler<FinancialPeriodNotFoundException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodNotFoundException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Financial period not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId
            });
    }
}