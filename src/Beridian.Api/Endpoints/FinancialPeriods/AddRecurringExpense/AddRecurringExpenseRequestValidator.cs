namespace Beridian.Api.Endpoints.FinancialPeriods.AddRecurringExpense;

internal static class AddRecurringExpenseRequestValidator
{
    public static Dictionary<string, string[]> Validate(AddRecurringExpenseRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = [ "Expense name is required." ];
        }

        if (request.PlannedAmount < 0)
        {
            errors["plannedAmount"] = [ "Planned amount must be greater than or equal to zero." ];
        }

        return errors;
    }
}