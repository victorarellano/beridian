using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.FinancialPeriods.Events;
using Beridian.Domain.Services;

namespace Beridian.Domain.Tests.FinancialPeriods.Events;

public sealed class FinancialPeriodDomainEventTests
{
    [Fact]
    public void Close_ShouldRegisterFinancialPeriodClosedEvent()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        var domainEvent = Assert.IsType<FinancialPeriodClosed>(Assert.Single(financialPeriod.DomainEvents));

        Assert.Equal(financialPeriod.Id, domainEvent.FinancialPeriodId);
        Assert.Equal(financialPeriod.Period, domainEvent.Period);
    }

    [Fact]
    public void Generate_ShouldRegisterFinancialPeriodGeneratedEvent()
    {
        var currentPeriod = CreateFinancialPeriod();
        var generator = new FinancialPeriodGenerator();

        var nextPeriod = generator.Generate(currentPeriod);

        var domainEvent = Assert.IsType<FinancialPeriodGenerated>(Assert.Single(nextPeriod.DomainEvents));

        Assert.Equal(currentPeriod.Id, domainEvent.SourceFinancialPeriodId);
        Assert.Equal(nextPeriod.Id, domainEvent.GeneratedFinancialPeriodId);
        Assert.Equal(nextPeriod.Period, domainEvent.GeneratedPeriod);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllRegisteredEvents()
    {
        var financialPeriod = CreateFinancialPeriod();

        financialPeriod.Close();

        financialPeriod.ClearDomainEvents();

        Assert.Empty(financialPeriod.DomainEvents);
    }

    private static FinancialPeriod CreateFinancialPeriod()
    {
        return FinancialPeriod.CreateInitial(Period.Create(2026, 8));
    }
}