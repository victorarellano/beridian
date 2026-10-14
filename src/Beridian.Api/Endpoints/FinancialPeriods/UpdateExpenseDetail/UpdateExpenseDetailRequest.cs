namespace Beridian.Api.Endpoints.FinancialPeriods.UpdateExpenseDetail;

public sealed record UpdateExpenseDetailRequest(string Description, decimal ActualAmount, DateOnly? TransactionDate = null, decimal? PlannedAmount = null);