namespace Beridian.Application.Incomes.EnterIncome;

public sealed record EnterIncomeCommand(Guid FinancialPeriodId, Guid IncomeId, decimal ActualAmount);