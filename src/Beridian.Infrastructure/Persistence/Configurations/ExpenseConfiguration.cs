using Beridian.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable(
            "expenses",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_expenses_expense_type",
                    "expense_type IN (1, 2, 3)");

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_status",
                    "status IN (1, 2)");

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_name",
                    "btrim(name) <> ''");

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_planned_amount",
                    "planned_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_actual_amount",
                    "actual_amount >= 0");

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_currency_values",
                    """
                    planned_amount_currency IN (1)
                    AND actual_amount_currency IN (1)
                    """);

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_currency_consistency",
                    "planned_amount_currency = actual_amount_currency");                    

                tableBuilder.HasCheckConstraint(
                    "ck_expenses_installments",
                    """
                    (
                        expense_type = 2
                        AND current_installment IS NOT NULL
                        AND total_installments IS NOT NULL
                        AND current_installment > 0
                        AND total_installments > 0
                        AND current_installment <= total_installments
                    )
                    OR
                    (
                        expense_type <> 2
                        AND current_installment IS NULL
                        AND total_installments IS NULL
                    )
                    """);
        });

        builder.HasKey(expense => expense.Id);

        builder.Property(expense => expense.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.HasDiscriminator<short>("expense_type")
            .HasValue<RecurringExpense>(1)
            .HasValue<FixedTermExpense>(2)
            .HasValue<DiscretionaryExpense>(3);

        builder.Property<short>("expense_type")
            .HasColumnName("expense_type")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(expense => expense.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(expense => expense.Status)
            .HasColumnName("status")
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();            

        builder.ComplexProperty(
            expense => expense.PlannedAmount,
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
            expense => expense.ActualAmount,
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
    
        builder.HasMany(expense => expense.Details)
            .WithOne()
            .HasForeignKey("expense_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(expense => expense.Details)
            .UsePropertyAccessMode(PropertyAccessMode.Field);    
    }
}