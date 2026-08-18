namespace Beridian.Application.FinancialPeriods.Exceptions;

public sealed class FinancialPeriodAlreadyExistsException : Exception
{
    public int Year { get; }

    public int Month { get; }

    public FinancialPeriodAlreadyExistsException(int year, int month, Exception? innerException = null)
        : base($"A financial period already exists for {year:D4}-{month:D2}.", innerException)
    {
        Year = year;
        Month = month;
    }
}