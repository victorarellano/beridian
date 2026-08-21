using Beridian.Domain.Incomes.Exceptions;

namespace Beridian.Api.ExceptionHandling.Incomes;

internal sealed class IncomeNotFoundInFinancialPeriodExceptionHandler
    : ApiExceptionHandler<IncomeNotFoundInFinancialPeriodException>
{
    protected override ApiProblem CreateProblem(IncomeNotFoundInFinancialPeriodException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Income not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId,
                ["incomeId"] = exception.IncomeId
            });
    }
}