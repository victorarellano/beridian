using Beridian.Domain.Common;

namespace Beridian.Application.Expenses.AddFixedTermExpense;
public sealed record AddFixedTermExpenseCommand(Guid FinancialPeriodId, string Name, Money plannedAmmount, int currentInstallment, int totalInstallments);