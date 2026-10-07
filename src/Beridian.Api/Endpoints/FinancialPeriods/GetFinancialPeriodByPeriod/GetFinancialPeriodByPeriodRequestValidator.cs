namespace Beridian.Api.Endpoints.FinancialPeriods.GetFinancialPeriodByPeriod;

internal static class GetFinancialPeriodByPeriodRequestValidator
{
    public static Dictionary<string, string[]> Validate(int year, int month)
    {
        var errors = new Dictionary<string, string[]>();

        if (year < 1 || year > 9999)
        {
            errors["year"] = ["Year must be between 1 and 9999."];
        }

        if (month < 1 || month > 12)
        {
            errors["month"] = ["Month must be between 1 and 12."];
        }

        return errors;
    }
}
