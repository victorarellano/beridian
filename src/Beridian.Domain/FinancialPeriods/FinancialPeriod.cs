using Beridian.Domain.Common;
using Beridian.Domain.Events;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods.Events;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    private readonly List<Expense> _expenses = [];
    private readonly List<Income> _incomes = [];
    private readonly List<Investment> _investments = [];
    private readonly List<IDomainEvent> _domainEvents = [];
    public Guid Id { get; private set; }
    public Period Period { get; private set; }
    public FinancialPeriodStatus Status { get; private set; }
    public TransferredBalance OpeningBalance { get; private set; }
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    public IReadOnlyCollection<Investment> Investments => _investments.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private FinancialPeriod()
    {
        Period = null!;
        OpeningBalance = null!;
    }

    private FinancialPeriod(Guid id, Period period, TransferredBalance openingBalance)
    {
        Id = id;
        Period = period;
        OpeningBalance = openingBalance;
        Status = FinancialPeriodStatus.Open;
    }

    public static FinancialPeriod Create(Period period, TransferredBalance openingBalance)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(openingBalance);

        return new FinancialPeriod(Guid.NewGuid(), period, openingBalance);
    }

    public static FinancialPeriod CreateInitial(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);

        return new FinancialPeriod(Guid.NewGuid(), period, TransferredBalance.Zero(Currency.Clp));
    }

    public FinancialPeriod GenerateNext()
    {
        var transferredBalance = TransferredBalance.Create(ActualBalance);

        var nextPeriod = Create(Period.Next(), transferredBalance);

        foreach (var income in _incomes)
        {
            nextPeriod.AddIncome(income.CopyToNextPeriod());
        }

        foreach (var expense in _expenses)
        {
            var copiedExpense = expense.CopyToNextPeriod();

            if (copiedExpense is not null)
            {
                nextPeriod.AddExpense(copiedExpense);
            }
        }

        return nextPeriod;
    }

    private void EnsureIsOpen()
    {
        if (Status == FinancialPeriodStatus.Closed)
        {
            throw new InvalidOperationException("A closed financial period cannot be modified.");
        }
    }

}


