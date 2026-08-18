namespace Beridian.Domain.FinancialPeriods;

public sealed record Period
{
    public int Year { get; private set; }

    public int Month { get; private set; }

    private Period()
    {
    }
    
    private Period(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static Period Create(int year, int month)
    {
        if (year <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero.");
        }

        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        return new Period(year, month);
    }

    public Period Next()
    {
        return Month == 12 ? Create(Year + 1, 1) : Create(Year, Month + 1);
    }

    public bool Contains(DateOnly date)
    {
        return date.Year == Year &&
            date.Month == Month;
    }
}