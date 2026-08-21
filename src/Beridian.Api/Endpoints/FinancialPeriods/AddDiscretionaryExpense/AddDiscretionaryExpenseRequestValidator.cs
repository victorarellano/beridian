namespace Beridian.Api.Endpoints.FinancialPeriods.AddDiscretionaryExpense;

internal static class AddDiscretionaryExpenseRequestValidator
{
    public static Dictionary<string, string[]> Validate(AddDiscretionaryExpenseRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = [ "Expense name is required." ];
        }

        return errors;
    }
}