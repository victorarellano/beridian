using Beridian.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beridian.Infrastructure.Persistence.Configurations;

public sealed class FixedTermExpenseConfiguration
    : IEntityTypeConfiguration<FixedTermExpense>
{
    public void Configure(EntityTypeBuilder<FixedTermExpense> builder)
    {
        builder.Property(expense => expense.CurrentInstallment)
            .HasColumnName("current_installment");

        builder.Property(expense => expense.TotalInstallments)
            .HasColumnName("total_installments");
    }
}