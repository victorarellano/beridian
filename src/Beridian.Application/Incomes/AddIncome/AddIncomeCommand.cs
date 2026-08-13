namespace Beridian.Application.Incomes.AddIncome;

public sealed record AddIncomeCommand(Guid FinancialPeriodId, string Name, decimal PlannedAmount);