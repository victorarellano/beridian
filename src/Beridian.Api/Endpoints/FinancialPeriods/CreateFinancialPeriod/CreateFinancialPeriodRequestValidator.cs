namespace Beridian.Api.Endpoints.FinancialPeriods.CreateFinancialPeriod;

internal static class CreateFinancialPeriodRequestValidator
{
    public static Dictionary<string, string[]> Validate(CreateFinancialPeriodRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (request.Year <= 0)
        {
            errors["year"] = ["Year must be greater than zero."];
        }

        if (request.Month is < 1 or > 12)
        {
            errors["month"] = ["Month must be between 1 and 12."];
        }

        return errors;
    }
}