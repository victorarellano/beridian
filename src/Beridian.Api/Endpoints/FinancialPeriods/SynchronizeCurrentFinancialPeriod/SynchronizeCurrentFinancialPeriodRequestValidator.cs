namespace Beridian.Api.Endpoints.FinancialPeriods.SynchronizeCurrentFinancialPeriod;

public sealed class SynchronizeCurrentFinancialPeriodRequestValidator
{
    public static Dictionary<string, string[]> Validate(SynchronizeCurrentFinancialPeriodRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.Year < 1 || request.Year > 9999)
        {
            errors["year"] = ["Year must be between 1 and 9999."];
        }

        if (request.Month < 1 || request.Month > 12)
        {
            errors["month"] = ["Month must be between 1 and 12."];
        }

        return errors;
    }
}