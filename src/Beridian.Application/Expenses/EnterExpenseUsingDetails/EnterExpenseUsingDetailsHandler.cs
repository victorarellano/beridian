using Beridian.Application.Abstractions.Persistence;

namespace Beridian.Application.Expenses.EnterExpenseUsingDetails;

public sealed class EnterExpenseUsingDetailsHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public EnterExpenseUsingDetailsHandler(
        IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnterExpenseUsingDetailsResult> HandleAsync(
        EnterExpenseUsingDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new InvalidOperationException("Financial period was not found.");
        }

        financialPeriod.EnterExpense(command.ExpenseId);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new EnterExpenseUsingDetailsResult(command.ExpenseId);
    }
}