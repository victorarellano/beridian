using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Infrastructure.Tests.TestData;

internal static class FinancialPeriodTestData
{
    public static FinancialPeriod CreateComplete()
    {
        var financialPeriod = FinancialPeriod.CreateInitial(
            Period.Create(2026, 8));

        var expense = RecurringExpense.Create(
            "Electricity",
            Money.Create(80_000m, Currency.Clp));

        financialPeriod.AddExpense(expense);

        financialPeriod.AddExpenseDetail(
            expense.Id,
            ExpenseDetail.Create(
                "August electricity bill",
                Money.Create(75_000m, Currency.Clp),
                new DateOnly(2026, 8, 10)));

        financialPeriod.EnterExpense(expense.Id);

        financialPeriod.AddIncome(
            Income.Create(
                "Salary",
                Money.Create(2_000_000m, Currency.Clp)));

        financialPeriod.AddInvestment(
            Investment.Create(
                "Monthly investment",
                Money.Create(300_000m, Currency.Clp)));

        return financialPeriod;
    }
}