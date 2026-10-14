using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Api.ExceptionHandling.Expenses;

internal sealed class ExpenseDetailNotFoundExceptionHandler : ApiExceptionHandler<ExpenseDetailNotFoundException>
{
    protected override ApiProblem CreateProblem(ExpenseDetailNotFoundException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Expense detail not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["expenseId"] = exception.ExpenseId,
                ["expenseDetailId"] = exception.ExpenseDetailId
            });
    }
}