using Beridian.Application.Abstractions.Persistence;
using Beridian.Application.FinancialPeriods.Exceptions;
using Beridian.Domain.Common;

namespace Beridian.Application.Investments.ConfirmInvestment;

public sealed class ConfirmInvestmentHandler
{
    private readonly IFinancialPeriodRepository _repository;

    public ConfirmInvestmentHandler(IFinancialPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConfirmInvestmentResult> HandleAsync(ConfirmInvestmentCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var financialPeriod = await _repository.GetByIdAsync(command.FinancialPeriodId, cancellationToken);

        if (financialPeriod is null)
        {
            throw new FinancialPeriodNotFoundException(command.FinancialPeriodId);
        }

        financialPeriod.ConfirmInvestment(command.InvestmentId, Money.Create(command.ActualAmount, Currency.Clp));

        await _repository.UpdateAsync(financialPeriod, cancellationToken);

        return new ConfirmInvestmentResult(command.InvestmentId);
    }
}