using Beridian.Application.Abstractions.Persistence;
using Beridian.Domain.Common;
using Beridian.Domain.Incomes;

namespace Beridian.Application.Incomes.AddIncome;

public sealed class AddIncomeHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddIncomeHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddIncomeResult> HandleAsync(
        AddIncomeCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(
                command.FinancialPeriodId,
                cancellationToken);

        if (financialPeriod is null)
        {
            throw new InvalidOperationException("Financial period was not found.");
        }

        var income = Income.Create(command.Name, Money.Create(command.PlannedAmount, Currency.Clp));
        financialPeriod.AddIncome(income);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddIncomeResult(income.Id);
    }
}