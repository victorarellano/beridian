namespace Beridian.Application.Exceptions;

public sealed class CurrentFinancialPeriodNotAvailableException : Exception
{
    public int Year { get; }
    public int Month { get; }

    public CurrentFinancialPeriodNotAvailableException(int year, int month)
        : base($"Financial period {year:D4}-{month:D2} " + "is not available.")
    {
        Year = year;
        Month = month;
    }
}