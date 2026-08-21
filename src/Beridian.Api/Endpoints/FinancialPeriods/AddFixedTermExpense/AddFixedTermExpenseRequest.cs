namespace Beridian.Api.Endpoints.FinancialPeriods.AddFixedTermExpense;

public sealed record AddFixedTermExpenseRequest(string Name, decimal PlannedAmount, int CurrentInstallment, int TotalInstallments);