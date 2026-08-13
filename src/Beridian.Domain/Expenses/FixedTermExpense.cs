using Beridian.Domain.Common;

namespace Beridian.Domain.Expenses;

public sealed class FixedTermExpense : Expense
{
    public int CurrentInstallment { get; private set; }

    public int TotalInstallments { get; private set; }

    private FixedTermExpense()
    {
    }

    private FixedTermExpense(Guid id, string name, Money plannedAmount, int currentInstallment, int totalInstallments)
        : base(id, name, plannedAmount)
    {
        if (currentInstallment <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentInstallment), "Current installment must be greater than zero.");
        }

        if (totalInstallments <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalInstallments), "Total installments must be greater than zero.");
        }

        if (currentInstallment > totalInstallments)
        {
            throw new ArgumentException(
                "Current installment cannot be greater than total installments.", nameof(currentInstallment));
        }

        CurrentInstallment = currentInstallment;
        TotalInstallments = totalInstallments;
    }

    public static FixedTermExpense Create(string name, Money plannedAmount, int currentInstallment, int totalInstallments)
    {
        return new FixedTermExpense(Guid.NewGuid(), name, plannedAmount, currentInstallment, totalInstallments);
    }

    internal override Expense? CopyToNextPeriod()
    {
        if (CurrentInstallment == TotalInstallments)
        {
            return null;
        }

        return Create(Name, ActualAmount, CurrentInstallment + 1, TotalInstallments);
    }
}