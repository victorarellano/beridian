using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;

namespace Beridian.Application.FinancialPeriods.GetFinancialPeriod;

internal static class GetFinancialPeriodResultMapper
{
    public static GetFinancialPeriodResult Map(FinancialPeriod financialPeriod)
    {
        ArgumentNullException.ThrowIfNull(financialPeriod);

        return new GetFinancialPeriodResult(
            financialPeriod.Id,
            financialPeriod.Period.Year,
            financialPeriod.Period.Month,
            financialPeriod.Status.ToString(),
            MapMoney(financialPeriod.OpeningBalance.Amount),
            MapMoney(financialPeriod.PlannedBalance),
            MapMoney(financialPeriod.ActualBalance),
            financialPeriod.Expenses
                .Select(MapExpense)
                .ToArray(),
            financialPeriod.Incomes
                .Select(MapIncome)
                .ToArray(),
            financialPeriod.Investments
                .Select(MapInvestment)
                .ToArray());
    }

    private static ExpenseResult MapExpense(Expense expense)
    {
        ( string Type, int? CurrentInstallment, int? TotalInstallments ) expenseData = expense 
        switch
        {
            RecurringExpense =>
                (
                    "Recurring",
                    null,
                    null
                ),

            FixedTermExpense fixedTermExpense =>
                (
                    "FixedTerm",
                    fixedTermExpense.CurrentInstallment,
                    fixedTermExpense.TotalInstallments
                ),

            DiscretionaryExpense =>
                (
                    "Discretionary",
                    null,
                    null
                ),

            _ => throw new InvalidOperationException(
                $"Unsupported expense type '{expense.GetType().Name}'.")
        };

        return new ExpenseResult(
            expense.Id,
            expenseData.Type,
            expense.Name,
            expense.Status.ToString(),
            MapMoney(expense.PlannedAmount),
            MapMoney(expense.ActualAmount),
            expenseData.CurrentInstallment,
            expenseData.TotalInstallments,
            expense.Details
                .Select(MapExpenseDetail)
                .ToArray());
    }

    private static ExpenseDetailResult MapExpenseDetail(ExpenseDetail expenseDetail)
    {
        return new ExpenseDetailResult(
            expenseDetail.Id,
            expenseDetail.Description,
            expenseDetail.TransactionDate,
            expenseDetail.PlannedAmount is null
                ? null
                : MapMoney(expenseDetail.PlannedAmount),
            MapMoney(expenseDetail.ActualAmount));
    }

    private static IncomeResult MapIncome(Income income)
    {
        return new IncomeResult(
            income.Id,
            income.Name,
            income.Status.ToString(),
            MapMoney(income.PlannedAmount),
            MapMoney(income.ActualAmount));
    }

    private static InvestmentResult MapInvestment(Investment investment)
    {
        return new InvestmentResult(
            investment.Id,
            investment.Name,
            investment.Status.ToString(),
            MapMoney(investment.PlannedAmount),
            MapMoney(investment.ActualAmount));
    }

    private static MoneyResult MapMoney(Money money)
    {
        return new MoneyResult(money.Amount, money.Currency.ToString());
    }
}