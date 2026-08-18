using Beridian.Domain.FinancialPeriods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class FinancialPeriodConfiguration
    : IEntityTypeConfiguration<FinancialPeriod>
{
    public void Configure(EntityTypeBuilder<FinancialPeriod> builder)
    {
        builder.ToTable("financial_periods",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_financial_periods_status",
                    "status IN (1, 2)");
                
                tableBuilder.HasCheckConstraint(
                    "ck_financial_periods_month",
                    "period_month BETWEEN 1 AND 12");

                tableBuilder.HasCheckConstraint(
                    "ck_financial_periods_year",
                    "period_year BETWEEN 1 AND 9999");
                
                tableBuilder.HasCheckConstraint(
                    "ck_financial_periods_opening_balance_currency",
                    "opening_balance_currency IN (1)");                    
            });

        builder.HasKey(financialPeriod => financialPeriod.Id);

        builder.Property(financialPeriod => financialPeriod.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(financialPeriod => financialPeriod.Status)
            .HasColumnName("status")
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

        builder.OwnsOne(
            financialPeriod => financialPeriod.Period,
            periodBuilder =>
            {
                periodBuilder.Property(period => period.Year)
                    .HasColumnName("period_year")
                    .IsRequired();

                periodBuilder.Property(period => period.Month)
                    .HasColumnName("period_month")
                    .IsRequired();

                periodBuilder.HasIndex(period => new
                    {
                        period.Year,
                        period.Month
                    })
                    .IsUnique()
                    .HasDatabaseName(
                        "ux_financial_periods_year_month");
            });

        builder.Navigation(financialPeriod => financialPeriod.Period)
            .IsRequired();            

        builder.ComplexProperty(
            financialPeriod => financialPeriod.OpeningBalance,
            transferredBalanceBuilder =>
            {
                transferredBalanceBuilder.ComplexProperty(
                    transferredBalance => transferredBalance.Amount,
                    moneyBuilder =>
                    {
                        moneyBuilder.Property(money => money.Amount)
                            .HasColumnName("opening_balance_amount")
                            .HasPrecision(18, 2)
                            .IsRequired();

                        moneyBuilder.Property(money => money.Currency)
                            .HasColumnName("opening_balance_currency")
                            .HasConversion<short>()
                            .HasColumnType("smallint")
                            .IsRequired();
                    });
            });                            

        builder.Ignore(financialPeriod => financialPeriod.DomainEvents);

        builder.HasMany(financialPeriod => financialPeriod.Expenses)
            .WithOne()
            .HasForeignKey("financial_period_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(financialPeriod => financialPeriod.Expenses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(financialPeriod => financialPeriod.Incomes)
            .WithOne()
            .HasForeignKey("financial_period_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(financialPeriod => financialPeriod.Incomes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(financialPeriod => financialPeriod.Investments)
            .WithOne()
            .HasForeignKey("financial_period_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(financialPeriod => financialPeriod.Investments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);                       

    }
}