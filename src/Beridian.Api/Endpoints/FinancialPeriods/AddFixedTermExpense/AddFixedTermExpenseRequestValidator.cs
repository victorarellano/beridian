namespace Beridian.Api.Endpoints.FinancialPeriods.AddFixedTermExpense;

internal static class AddFixedTermExpenseRequestValidator
{
    public static Dictionary<string, string[]> Validate(AddFixedTermExpenseRequest request)
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

        if (request.CurrentInstallment <= 0)
        {
            errors["currentInstallment"] = [ "Current installment must be greater than zero." ];
        }

        if (request.TotalInstallments <= 0)
        {
            errors["totalInstallments"] = [ "Total installments must be greater than zero." ];
        }

        if (request.CurrentInstallment > 0 &&
            request.TotalInstallments > 0 &&
            request.CurrentInstallment > request.TotalInstallments)
        {
            errors["currentInstallment"] = [ "Current installment cannot be greater than total installments." ];
        }

        return errors;
    }
}