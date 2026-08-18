using Beridian.Domain.Investments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class InvestmentConfiguration
    : IEntityTypeConfiguration<Investment>
{
    public void Configure(EntityTypeBuilder<Investment> builder)
    {
        builder.ToTable(
            "investments",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_investments_name",
                    "btrim(name) <> ''");

                tableBuilder.HasCheckConstraint(
                    "ck_investments_status",
                    "status IN (1, 2)");

                tableBuilder.HasCheckConstraint(
                    "ck_investments_planned_amount",
                    "planned_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_investments_actual_amount",
                    "actual_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_investments_currency_values",
                    """
                    planned_amount_currency IN (1)
                    AND actual_amount_currency IN (1)
                    """);                    

                tableBuilder.HasCheckConstraint(
                    "ck_investments_currency_consistency",
                    "planned_amount_currency = actual_amount_currency");
            });

        builder.HasKey(investment => investment.Id);

        builder.Property(investment => investment.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(investment => investment.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(investment => investment.Status)
            .HasColumnName("status")
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

        builder.ComplexProperty(
            investment => investment.PlannedAmount,
            moneyBuilder =>
            {
                moneyBuilder.Property(money => money.Amount)
                    .HasColumnName("planned_amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                moneyBuilder.Property(money => money.Currency)
                    .HasColumnName("planned_amount_currency")
                    .HasConversion<short>()
                    .HasColumnType("smallint")
                    .IsRequired();
            });

        builder.ComplexProperty(
            investment => investment.ActualAmount,
            moneyBuilder =>
            {
                moneyBuilder.Property(money => money.Amount)
                    .HasColumnName("actual_amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                moneyBuilder.Property(money => money.Currency)
                    .HasColumnName("actual_amount_currency")
                    .HasConversion<short>()
                    .HasColumnType("smallint")
                    .IsRequired();
            });
    }
}