namespace Beridian.Api.Endpoints.FinancialPeriods.AddExpenseDetail;

public sealed record AddExpenseDetailRequest(
    string Description,
    decimal ActualAmount,
    DateOnly? TransactionDate = null,
    decimal? PlannedAmount = null);