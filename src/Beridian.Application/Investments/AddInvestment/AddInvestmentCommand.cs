namespace Beridian.Application.Investments.AddInvestment;

public sealed record AddInvestmentCommand(
    Guid FinancialPeriodId,
    string Name,
    decimal PlannedAmount);