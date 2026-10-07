namespace Beridian.Application.FinancialPeriods.Exceptions;

public sealed class FinancialPeriodByPeriodNotFoundException : Exception
{
    public int Year { get; }
    public int Month { get; }

    public FinancialPeriodByPeriodNotFoundException(int year, int month)
        : base($"Financial period '{year}-{month:D2}' was not found.")
    {
        Year = year;
        Month = month;
    }
}