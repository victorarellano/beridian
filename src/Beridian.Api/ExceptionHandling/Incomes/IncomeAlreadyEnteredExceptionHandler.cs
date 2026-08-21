using Beridian.Domain.Incomes.Exceptions;

namespace Beridian.Api.ExceptionHandling.Incomes;

internal sealed class IncomeAlreadyEnteredExceptionHandler
    : ApiExceptionHandler<IncomeAlreadyEnteredException>
{
    protected override ApiProblem CreateProblem(
        IncomeAlreadyEnteredException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Income already entered",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["incomeId"] = exception.IncomeId
            });
    }
}