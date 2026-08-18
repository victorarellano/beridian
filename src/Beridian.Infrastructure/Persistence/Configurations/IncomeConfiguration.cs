using Beridian.Domain.Incomes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable(
            "incomes",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_incomes_name",
                    "btrim(name) <> ''");

                tableBuilder.HasCheckConstraint(
                    "ck_incomes_status",
                    "status IN (1, 2)");

                tableBuilder.HasCheckConstraint(
                    "ck_incomes_planned_amount",
                    "planned_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_incomes_actual_amount",
                    "actual_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_incomes_currency_values",
                    """
                    planned_amount_currency IN (1)
                    AND actual_amount_currency IN (1)
                    """);

                tableBuilder.HasCheckConstraint(
                    "ck_incomes_currency_consistency",
                    "planned_amount_currency = actual_amount_currency");
            });

        builder.HasKey(income => income.Id);

        builder.Property(income => income.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(income => income.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(income => income.Status)
            .HasColumnName("status")
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

        builder.ComplexProperty(
            income => income.PlannedAmount,
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
            income => income.ActualAmount,
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