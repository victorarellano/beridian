using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Api.ExceptionHandling.Expenses;

internal sealed class ExpenseAlreadyEnteredExceptionHandler
    : ApiExceptionHandler<ExpenseAlreadyEnteredException>
{
    protected override ApiProblem CreateProblem(ExpenseAlreadyEnteredException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Expense already entered",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["expenseId"] = exception.ExpenseId
            });
    }
}