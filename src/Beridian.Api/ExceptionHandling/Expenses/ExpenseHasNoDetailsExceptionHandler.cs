using Beridian.Domain.Expenses.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Beridian.Api.ExceptionHandling;

internal sealed class ExpenseHasNoDetailsExceptionHandler : ApiExceptionHandler<ExpenseHasNoDetailsException>
{
    protected override ApiProblem CreateProblem(ExpenseHasNoDetailsException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Expense has no details",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["expenseId"] = exception.ExpenseId
            });
    }
}