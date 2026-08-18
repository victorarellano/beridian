using Beridian.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class ExpenseDetailConfiguration
    : IEntityTypeConfiguration<ExpenseDetail>
{
    public void Configure(EntityTypeBuilder<ExpenseDetail> builder)
    {
        builder.ToTable(
            "expense_details",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_description",
                    "btrim(description) <> ''");

                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_actual_amount",
                    "actual_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_planned_amount",
                    "planned_amount IS NULL OR planned_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_planned_amount_completeness",
                    """
                    (
                        planned_amount IS NULL
                        AND planned_amount_currency IS NULL
                    )
                    OR
                    (
                        planned_amount IS NOT NULL
                        AND planned_amount_currency IS NOT NULL
                    )
                    """);

                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_currency_values",
                    """
                    actual_amount_currency IN (1)
                    AND
                    (
                        planned_amount_currency IS NULL
                        OR planned_amount_currency IN (1)
                    )
                    """);

                tableBuilder.HasCheckConstraint(
                    "ck_expense_details_currency_consistency",
                    """
                    planned_amount_currency IS NULL
                    OR planned_amount_currency = actual_amount_currency
                    """);
        });

        builder.HasKey(detail => detail.Id);

        builder.Property(detail => detail.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(detail => detail.Description)
            .HasColumnName("description")
            .IsRequired();

        builder.OwnsOne(detail => detail.PlannedAmount, moneyBuilder => {
            moneyBuilder.Property(money => money.Amount)
                .HasColumnName("planned_amount")
                .HasPrecision(18, 2);

            moneyBuilder.Property(money => money.Currency)
                .HasColumnName("planned_amount_currency")
                .HasConversion<short>()
                .HasColumnType("smallint");
        });

        builder.ComplexProperty(detail => detail.ActualAmount, moneyBuilder => {
            moneyBuilder.Property(money => money.Amount)
                .HasColumnName("actual_amount")
                .HasPrecision(18, 2);

            moneyBuilder.Property(money => money.Currency)
                .HasColumnName("actual_amount_currency")
                .HasConversion<short>()
                .HasColumnType("smallint");
        });                     

        builder.Property(detail => detail.TransactionDate)
            .HasColumnName("transaction_date")
            .HasColumnType("date");
    }
}