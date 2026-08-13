using Beridian.Domain.Common;

namespace Beridian.Domain.FinancialPeriods;

public sealed partial class FinancialPeriod
{
    public Money TotalPlannedIncome => SumMoney(_incomes.Select(income => income.PlannedAmount));
    public Money TotalActualIncome => SumMoney(_incomes.Select(income => income.ActualAmount));
    public Money TotalPlannedExpenses => SumMoney(_expenses.Select(expense => expense.PlannedAmount));
    public Money TotalActualExpenses => SumMoney(_expenses.Select(expense => expense.ActualAmount));
    public Money TotalPlannedInvestments => SumMoney(_investments.Select(investment => investment.PlannedAmount));
    public Money TotalActualInvestments => SumMoney(_investments.Select(investment => investment.ActualAmount));
    public Money PlannedBalance => OpeningBalance.Amount.Add(TotalPlannedIncome).Subtract(TotalPlannedExpenses).Subtract(TotalPlannedInvestments);
    public Money ActualBalance => OpeningBalance.Amount.Add(TotalActualIncome).Subtract(TotalActualExpenses).Subtract(TotalActualInvestments);

    private static Money SumMoney(IEnumerable<Money> amounts)
    {
        var total = Money.Zero(Currency.Clp);

        foreach (var amount in amounts)
        {
            total = total.Add(amount);
        }

        return total;
    }
}
