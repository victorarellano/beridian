namespace Beridian.Api.Endpoints.FinancialPeriods.EnterIncome;

internal static class EnterIncomeRequestValidator
{
    public static Dictionary<string, string[]> Validate(EnterIncomeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (request.ActualAmount < 0)
        {
            errors["actualAmount"] = [ "Actual amount must be greater than or equal to zero." ];
        }

        return errors;
    }
}