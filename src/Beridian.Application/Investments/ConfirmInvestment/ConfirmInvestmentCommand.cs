namespace Beridian.Application.Investments.ConfirmInvestment;

public sealed record ConfirmInvestmentCommand(
    Guid FinancialPeriodId,
    Guid InvestmentId,
    decimal ActualAmount);