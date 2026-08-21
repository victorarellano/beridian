namespace Beridian.Domain.Incomes.Exceptions;

public sealed class IncomeAlreadyEnteredException
    : InvalidOperationException
{
    public Guid IncomeId { get; }

    public IncomeAlreadyEnteredException(Guid incomeId)
        : base($"Income '{incomeId}' has already been entered.")
    {
        IncomeId = incomeId;
    }
}