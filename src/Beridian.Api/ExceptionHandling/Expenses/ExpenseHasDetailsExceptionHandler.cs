using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Api.ExceptionHandling;

internal sealed class ExpenseHasDetailsExceptionHandler : ApiExceptionHandler<ExpenseHasDetailsException>
{
    protected override ApiProblem CreateProblem(
        ExpenseHasDetailsException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Expense has details",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["expenseId"] = exception.ExpenseId
            });
    }
}