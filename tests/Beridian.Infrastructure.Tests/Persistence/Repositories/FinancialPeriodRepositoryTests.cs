using Beridian.Domain.Common;
using Beridian.Domain.Expenses;
using Beridian.Domain.FinancialPeriods;
using Beridian.Domain.Incomes;
using Beridian.Domain.Investments;
using Beridian.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Beridian.Infrastructure.Tests.Persistence.Repositories;

[Collection(PostgreSqlCollection.Name)]
public sealed class FinancialPeriodRepositoryTests
{
    private readonly PostgreSqlFixture _fixture;

    public FinancialPeriodRepositoryTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_WhenFinancialPeriodIsValid_ShouldPersistFinancialPeriod()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();

        var repository = new FinancialPeriodRepository(dbContext);

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        // Act
        await repository.AddAsync(financialPeriod);

        // Assert
        await using var verificationDbContext = _fixture.CreateDbContext();

        var persistedFinancialPeriod = await verificationDbContext.FinancialPeriods
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    candidate =>
                        candidate.Id == financialPeriod.Id);

        Assert.NotNull(persistedFinancialPeriod);
        Assert.Equal(financialPeriod.Id, persistedFinancialPeriod.Id);
        Assert.Equal(financialPeriod.Period, persistedFinancialPeriod.Period);
    }

    [Fact]
    public async Task GetByIdAsync_WhenFinancialPeriodExists_ShouldReturnCompleteAggregate()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        var recurringExpense = RecurringExpense.Create("Internet", Money.Create(30_000m, Currency.Clp));

        var expenseDetail = ExpenseDetail.Create(
            "Fiber plan", 
            Money.Create(29_500m, Currency.Clp),
            new DateOnly(2026, 8, 5),
            Money.Create(30_000m, Currency.Clp));

        var fixedTermExpense = FixedTermExpense.Create(
            "Laptop",
            Money.Create(100_000m, Currency.Clp),
            currentInstallment: 2,
            totalInstallments: 6);

        var discretionaryExpense = DiscretionaryExpense.Create(
            "Personal expenses",
            Currency.Clp);

        var income = Income.Create(
            "Salary",
            Money.Create(2_000_000m, Currency.Clp));

        var investment = Investment.Create(
            "Emergency fund",
            Money.Create(200_000m, Currency.Clp));

        financialPeriod.AddExpense(recurringExpense);
        financialPeriod.AddExpenseDetail(
            recurringExpense.Id,
            expenseDetail);
        financialPeriod.EnterExpense(recurringExpense.Id);

        financialPeriod.AddExpense(fixedTermExpense);
        financialPeriod.EnterExpense(
            fixedTermExpense.Id,
            Money.Create(100_000m, Currency.Clp));

        financialPeriod.AddExpense(discretionaryExpense);
        financialPeriod.EnterExpense(
            discretionaryExpense.Id,
            Money.Create(20_000m, Currency.Clp));

        financialPeriod.AddIncome(income);
        financialPeriod.EnterIncome(
            income.Id,
            Money.Create(2_050_000m, Currency.Clp));

        financialPeriod.AddInvestment(investment);
        financialPeriod.ConfirmInvestment(
            investment.Id,
            Money.Create(180_000m, Currency.Clp));

        await using (var writeDbContext =
            _fixture.CreateDbContext())
        {
            var writeRepository =
                new FinancialPeriodRepository(writeDbContext);

            await writeRepository.AddAsync(financialPeriod);
        }

        // Act
        await using var readDbContext = _fixture.CreateDbContext();

        var readRepository = new FinancialPeriodRepository(readDbContext);

        var result = await readRepository.GetByIdAsync(financialPeriod.Id);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(financialPeriod.Id, result.Id);
        Assert.Equal(2026, result.Period.Year);
        Assert.Equal(8, result.Period.Month);

        Assert.Equal(3, result.Expenses.Count);
        Assert.Single(result.Incomes);
        Assert.Single(result.Investments);

        var persistedRecurringExpense =
            Assert.IsType<RecurringExpense>(
                result.Expenses.Single(
                    expense =>
                        expense.Id == recurringExpense.Id));

        var persistedDetail = Assert.Single(persistedRecurringExpense.Details);

        Assert.Equal(expenseDetail.Id, persistedDetail.Id);
        Assert.Equal(expenseDetail.Description, persistedDetail.Description);
        Assert.Equal(expenseDetail.TransactionDate, persistedDetail.TransactionDate);
        Assert.Equal(29_500m, persistedDetail.ActualAmount.Amount);

        var persistedFixedTermExpense =
            Assert.IsType<FixedTermExpense>(
                result.Expenses.Single(
                    expense =>
                        expense.Id == fixedTermExpense.Id));

        Assert.Equal(2, persistedFixedTermExpense.CurrentInstallment);
        Assert.Equal(6, persistedFixedTermExpense.TotalInstallments);
        Assert.Equal(100_000m, persistedFixedTermExpense.ActualAmount.Amount);

        var persistedDiscretionaryExpense =
            Assert.IsType<DiscretionaryExpense>(
                result.Expenses.Single(
                    expense =>
                        expense.Id == discretionaryExpense.Id));

        Assert.Equal(20_000m, persistedDiscretionaryExpense.ActualAmount.Amount);

        var persistedIncome = Assert.Single(result.Incomes);

        Assert.Equal(income.Id, persistedIncome.Id);
        Assert.Equal(2_050_000m, persistedIncome.ActualAmount.Amount);

        var persistedInvestment = Assert.Single(result.Investments);

        Assert.Equal(investment.Id, persistedInvestment.Id);
        Assert.Equal(180_000m, persistedInvestment.ActualAmount.Amount);
    }

    [Fact]
    public async Task GetByIdAsync_WhenFinancialPeriodDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();

        var repository = new FinancialPeriodRepository(dbContext);

        var financialPeriodId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(financialPeriodId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenTrackedFinancialPeriodIsModified_ShouldPersistChanges()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        await using (var seedDbContext = _fixture.CreateDbContext())
        {
            var seedRepository = new FinancialPeriodRepository(seedDbContext);

            await seedRepository.AddAsync(financialPeriod);
        }

        await using (var updateDbContext = _fixture.CreateDbContext())
        {
            var updateRepository = new FinancialPeriodRepository(updateDbContext);

            var persistedFinancialPeriod = await updateRepository.GetByIdAsync(financialPeriod.Id);

            Assert.NotNull(persistedFinancialPeriod);

            persistedFinancialPeriod.AddIncome(
                Income.Create(
                    "Salary",
                    Money.Create(
                        2_000_000m,
                        Currency.Clp)));

            // Act
            await updateRepository.UpdateAsync(persistedFinancialPeriod);
        }

        // Assert
        await using var verificationDbContext = _fixture.CreateDbContext();

        var verificationRepository = new FinancialPeriodRepository(verificationDbContext);

        var result = await verificationRepository.GetByIdAsync(financialPeriod.Id);

        Assert.NotNull(result);

        var persistedIncome = Assert.Single(result.Incomes);

        Assert.Equal("Salary", persistedIncome.Name);
        Assert.Equal(2_000_000m, persistedIncome.PlannedAmount.Amount);
    }

    [Fact]
    public async Task UpdateAsync_WhenFinancialPeriodIsDetached_ShouldThrowInvalidOperationException()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();

        var repository = new FinancialPeriodRepository(dbContext);

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        // Act
        var action = async () => await repository.UpdateAsync(financialPeriod);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }    

    [Fact]
    public async Task Insert_WhenOpeningBalanceCurrencyIsInvalid_ShouldThrowPostgresException()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();

        var financialPeriodId = Guid.NewGuid();

        // Act
        var action = async () =>
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"""
                INSERT INTO financial_periods
                (
                    id,
                    status,
                    period_year,
                    period_month,
                    opening_balance_amount,
                    opening_balance_currency
                )
                VALUES
                (
                    {financialPeriodId},
                    {(short)1},
                    {2026},
                    {8},
                    {0m},
                    {(short)99}
                )
                """);

        // Assert
        var exception = await Assert.ThrowsAsync<PostgresException>(action);

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);

        Assert.Equal("ck_financial_periods_opening_balance_currency", exception.ConstraintName);
    }

    [Theory]
    [InlineData(
        99,
        2026,
        8,
        "ck_financial_periods_status")]
    [InlineData(
        1,
        2026,
        13,
        "ck_financial_periods_month")]
    [InlineData(
        1,
        0,
        8,
        "ck_financial_periods_year")]
    public async Task Insert_WhenFinancialPeriodValueIsInvalid_ShouldViolateExpectedConstraint(
        int status,
        int year,
        int month,
        string expectedConstraint)
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();

        // Act
        var action = async () =>
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"""
                INSERT INTO financial_periods
                (
                    id,
                    status,
                    period_year,
                    period_month,
                    opening_balance_amount,
                    opening_balance_currency
                )
                VALUES
                (
                    {Guid.NewGuid()},
                    {(short)status},
                    {year},
                    {month},
                    {0m},
                    {(short)1}
                )
                """);

        // Assert
        var exception = await Assert.ThrowsAsync<PostgresException>(action);

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);

        Assert.Equal(expectedConstraint, exception.ConstraintName);
    }

    [Fact]
    public async Task Insert_WhenFixedTermInstallmentsAreInvalid_ShouldViolateInstallmentsConstraint()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var financialPeriod = FinancialPeriod.CreateInitial(Period.Create(2026, 8));

        await using (var seedDbContext = _fixture.CreateDbContext())
        {
            var repository = new FinancialPeriodRepository(seedDbContext);

            await repository.AddAsync(financialPeriod);
        }

        await using var dbContext = _fixture.CreateDbContext();

        // Act
        var action = async () =>
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"""
                INSERT INTO expenses
                (
                    id,
                    financial_period_id,
                    expense_type,
                    name,
                    status,
                    planned_amount,
                    planned_amount_currency,
                    actual_amount,
                    actual_amount_currency,
                    current_installment,
                    total_installments
                )
                VALUES
                (
                    {Guid.NewGuid()},
                    {financialPeriod.Id},
                    {(short)2},
                    {"Laptop"},
                    {(short)1},
                    {100_000m},
                    {(short)1},
                    {0m},
                    {(short)1},
                    {13},
                    {12}
                )
                """);

        // Assert
        var exception = await Assert.ThrowsAsync<PostgresException>(action);

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);

        Assert.Equal("ck_expenses_installments", exception.ConstraintName);
    }

}