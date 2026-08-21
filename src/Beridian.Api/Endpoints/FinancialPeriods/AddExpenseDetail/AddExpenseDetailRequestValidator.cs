namespace Beridian.Api.Endpoints.FinancialPeriods.AddExpenseDetail;

internal static class AddExpenseDetailRequestValidator
{
    public static Dictionary<string, string[]> Validate(AddExpenseDetailRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            errors["description"] = [ "Expense detail description is required." ];
        }

        if (request.ActualAmount < 0)
        {
            errors["actualAmount"] = [ "Actual amount must be greater than or equal to zero." ];
        }

        if (request.PlannedAmount < 0)
        {
            errors["plannedAmount"] = [ "Planned amount must be greater than or equal to zero." ];
        }

        return errors;
    }
}