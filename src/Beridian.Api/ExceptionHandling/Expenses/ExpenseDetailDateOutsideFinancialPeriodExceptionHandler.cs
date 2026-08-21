using Beridian.Domain.Expenses.Exceptions;

namespace Beridian.Api.ExceptionHandling.Expenses;

internal sealed class ExpenseDetailDateOutsideFinancialPeriodExceptionHandler
    : ApiExceptionHandler<ExpenseDetailDateOutsideFinancialPeriodException>
{
    protected override ApiProblem CreateProblem(ExpenseDetailDateOutsideFinancialPeriodException exception)
    {
        return new ApiProblem(
            StatusCodes.Status400BadRequest,
            "Expense detail date is invalid",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["financialPeriodId"] = exception.FinancialPeriodId,

                ["expenseId"] = exception.ExpenseId,

                ["transactionDate"] = exception.TransactionDate,

                ["year"] = exception.Year,

                ["month"] = exception.Month
            });
    }
}