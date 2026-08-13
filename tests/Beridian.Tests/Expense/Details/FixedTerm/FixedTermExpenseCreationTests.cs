using Beridian.Domain.Common;
using Beridian.Domain.Expenses;

namespace Beridian.Domain.Tests.Expenses.FixedTerm;

public sealed class FixedTermExpenseCreationTests
{
    [Fact]
    public void Create_WithValidInstallments_ShouldCreateFixedTermExpense()
    {
        var expense = FixedTermExpense.Create("Laptop", Money.Create(100_000m, Currency.Clp), currentInstallment: 2, totalInstallments: 6);

        Assert.NotEqual(Guid.Empty, expense.Id);
        Assert.Equal(2, expense.CurrentInstallment);
        Assert.Equal(6, expense.TotalInstallments);
        Assert.Equal(ExpenseStatus.Created, expense.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidCurrentInstallment_ShouldThrowArgumentOutOfRangeException(int currentInstallment)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => FixedTermExpense.Create(
                "Laptop", Money.Create(100_000m, Currency.Clp), currentInstallment, 6));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidTotalInstallments_ShouldThrowArgumentOutOfRangeException(
        int totalInstallments)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => FixedTermExpense.Create("Laptop", Money.Create(100_000m, Currency.Clp), 1, totalInstallments));
    }

    [Fact]
    public void Create_WhenCurrentInstallmentExceedsTotal_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => FixedTermExpense.Create("Laptop", Money.Create(100_000m, Currency.Clp), currentInstallment: 7, totalInstallments: 6));
    }
}