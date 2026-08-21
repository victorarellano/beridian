using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;
using Beridian.Domain.FinancialPeriods.Exceptions;
using Beridian.Domain.Investments;

namespace Beridian.Application.Investments.AddInvestment;

public sealed class AddInvestmentHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public AddInvestmentHandler(
        IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddInvestmentResult> HandleAsync(
        AddInvestmentCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        var investment = Investment.Create(command.Name, Money.Create(command.PlannedAmount, Currency.Clp));

        financialPeriod.AddInvestment(investment);

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new AddInvestmentResult(investment.Id);
    }
}