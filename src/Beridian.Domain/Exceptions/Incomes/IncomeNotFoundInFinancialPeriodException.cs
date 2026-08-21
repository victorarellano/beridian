namespace Beridian.Domain.Incomes.Exceptions;

public sealed class IncomeNotFoundInFinancialPeriodException : InvalidOperationException
{
    public Guid FinancialPeriodId { get; }

    public Guid IncomeId { get; }

    public IncomeNotFoundInFinancialPeriodException(Guid financialPeriodId, Guid incomeId)
        : base($"Income '{incomeId}' was not found in financial period '{financialPeriodId}'.")
    {
        FinancialPeriodId = financialPeriodId;
        IncomeId = incomeId;
    }
}