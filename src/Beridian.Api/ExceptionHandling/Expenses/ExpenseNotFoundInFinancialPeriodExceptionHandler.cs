using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Api.ExceptionHandling.Expenses;

internal sealed class ExpenseNotFoundInFinancialPeriodExceptionHandler
    : ApiExceptionHandler<ExpenseNotFoundInFinancialPeriodException>
{
    protected override ApiProblem CreateProblem(ExpenseNotFoundInFinancialPeriodException exception)
    {
        return new ApiProblem(
            StatusCodes.Status404NotFound,
            "Expense not found",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId,

                ["expenseId"] = exception.ExpenseId
            });
    }
}