using Beridian.Domain.FinancialPeriods.Exceptions;

namespace Beridian.Api.ExceptionHandling.FinancialPeriods;

internal sealed class FinancialPeriodCannotBeClosedExceptionHandler
    : ApiExceptionHandler<FinancialPeriodCannotBeClosedException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodCannotBeClosedException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Financial period cannot be closed",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId,
                ["reason"] = exception.Reason.ToString()
            });
    }
}