namespace Beridian.Api.Endpoints.FinancialPeriods.AddIncome;

internal static class AddIncomeRequestValidator
{
    public static Dictionary<string, string[]> Validate(AddIncomeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = [ "Income name is required." ];
        }

        if (request.PlannedAmount < 0)
        {
            errors["plannedAmount"] = [ "Planned amount must be greater than or equal to zero." ];
        }

        return errors;
    }
}