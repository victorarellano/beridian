namespace Beridian.Domain.FinancialPeriods.Exceptions;

public sealed class FinancialPeriodCannotBeClosedException : InvalidOperationException
{
    public Guid FinancialPeriodId { get; }

    public FinancialPeriodClosingFailureReason Reason { get; }

    public FinancialPeriodCannotBeClosedException(Guid financialPeriodId, FinancialPeriodClosingFailureReason reason)
        : base(CreateMessage(reason))
    {
        FinancialPeriodId = financialPeriodId;
        Reason = reason;
    }

    private static string CreateMessage(FinancialPeriodClosingFailureReason reason)
    {
        return reason switch
        {
            FinancialPeriodClosingFailureReason.UnenteredExpenses => "The financial period cannot be closed while expenses remain unentered.",
            FinancialPeriodClosingFailureReason.UnenteredIncomes => "The financial period cannot be closed while incomes remain unentered.",
            _ => throw new ArgumentOutOfRangeException(
                nameof(reason),
                reason,
                "Unsupported financial period closing failure reason.")
        };
    }
}