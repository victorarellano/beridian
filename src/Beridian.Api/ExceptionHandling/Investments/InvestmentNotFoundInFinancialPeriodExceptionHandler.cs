using Beridian.Domain.Investments.Exceptions;

namespace Beridian.Api.ExceptionHandling;

internal sealed class InvestmentNotFoundInFinancialPeriodExceptionHandler 
    : ApiExceptionHandler<InvestmentNotFoundInFinancialPeriodException>
{
    protected override ApiProblem CreateProblem(InvestmentNotFoundInFinancialPeriodException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Income not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId,
                ["investmentId"] = exception.InvestmentId
            });
    }
}