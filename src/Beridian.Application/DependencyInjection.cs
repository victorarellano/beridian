using Beridian.Application.Expenses.AddDiscretionaryExpense;
using Beridian.Application.Expenses.AddExpenseDetail;
using Beridian.Application.Expenses.AddFixedTermExpense;
using Beridian.Application.Expenses.AddRecurringExpense;
using Beridian.Application.Expenses.EnterExpense;
using Beridian.Application.Expenses.EnterExpenseUsingDetails;
using Beridian.Application.FinancialPeriods.CloseFinancialPeriod;
using Beridian.Application.FinancialPeriods.CreateFinancialPeriod;
using Beridian.Application.FinancialPeriods.GenerateNextFinancialPeriod;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Beridian.Application.Incomes.AddIncome;
using Beridian.Application.Incomes.EnterIncome;
using Beridian.Application.Investments.AddInvestment;
using Beridian.Application.Investments.ConfirmInvestment;
using Beridian.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Beridian.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateFinancialPeriodHandler>();
        services.AddScoped<GenerateNextFinancialPeriodHandler>();
        services.AddScoped<CloseFinancialPeriodHandler>();
        services.AddScoped<GetFinancialPeriodHandler>();

        services.AddScoped<AddRecurringExpenseHandler>();
        services.AddScoped<AddDiscretionaryExpenseHandler>();
        services.AddScoped<AddFixedTermExpenseHandler>();
        services.AddScoped<AddExpenseDetailHandler>();
        services.AddScoped<EnterExpenseHandler>();
        services.AddScoped<EnterExpenseUsingDetailsHandler>();

        services.AddScoped<AddIncomeHandler>();
        services.AddScoped<EnterIncomeHandler>();

        services.AddScoped<AddInvestmentHandler>();
        services.AddScoped<ConfirmInvestmentHandler>();

        services.AddScoped<FinancialPeriodGenerator>();

        return services;

    }
}