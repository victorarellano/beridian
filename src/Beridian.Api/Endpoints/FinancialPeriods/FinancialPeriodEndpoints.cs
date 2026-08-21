using Beridian.Api.Endpoints.FinancialPeriods.AddDiscretionaryExpense;
using Beridian.Api.Endpoints.FinancialPeriods.AddExpenseDetail;
using Beridian.Api.Endpoints.FinancialPeriods.AddFixedTermExpense;
using Beridian.Api.Endpoints.FinancialPeriods.AddIncome;
using Beridian.Api.Endpoints.FinancialPeriods.AddInvestment;
using Beridian.Api.Endpoints.FinancialPeriods.AddRecurringExpense;
using Beridian.Api.Endpoints.FinancialPeriods.CloseFinancialPeriod;
using Beridian.Api.Endpoints.FinancialPeriods.ConfirmInvestment;
using Beridian.Api.Endpoints.FinancialPeriods.CreateFinancialPeriod;
using Beridian.Api.Endpoints.FinancialPeriods.EnterExpense;
using Beridian.Api.Endpoints.FinancialPeriods.EnterExpenseUsingDetails;
using Beridian.Api.Endpoints.FinancialPeriods.EnterIncome;
using Beridian.Api.Endpoints.FinancialPeriods.GenerateNextFinancialPeriod;
using Beridian.Api.Endpoints.FinancialPeriods.GetFinancialPeriod;
using Beridian.Api.Versioning;

namespace Beridian.Api.Endpoints.FinancialPeriods;

public static class FinancialPeriodEndpoints
{
    public static IEndpointRouteBuilder MapFinancialPeriodEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(ApiVersions.V1)
            .ReportApiVersions()
            .Build();

        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/financial-periods")
            .WithApiVersionSet(versionSet)
            .WithTags("Financial Periods");

        CreateFinancialPeriodEndpoint.Map(group);
        GetFinancialPeriodEndpoint.Map(group);
        CloseFinancialPeriodEndpoint.Map(group);

        GenerateNextFinancialPeriodEndpoint.Map(group);

        AddIncomeEndpoint.Map(group);
        EnterIncomeEndpoint.Map(group);
        
        AddRecurringExpenseEndpoint.Map(group);
        AddFixedTermExpenseEndpoint.Map(group);
        AddDiscretionaryExpenseEndpoint.Map(group);
        AddExpenseDetailEndpoint.Map(group);

        EnterExpenseEndpoint.Map(group);
        EnterExpenseUsingDetailsEndpoint.Map(group);

        AddInvestmentEndpoint.Map(group);
        ConfirmInvestmentEndpoint.Map(group);

        return endpoints;
    }
}