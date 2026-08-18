using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beridian.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "financial_periods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_year = table.Column<int>(type: "integer", nullable: false),
                    period_month = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    opening_balance_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    opening_balance_currency = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_periods", x => x.id);
                    table.CheckConstraint("ck_financial_periods_month", "period_month BETWEEN 1 AND 12");
                    table.CheckConstraint("ck_financial_periods_opening_balance_currency", "opening_balance_currency IN (1)");
                    table.CheckConstraint("ck_financial_periods_status", "status IN (1, 2)");
                    table.CheckConstraint("ck_financial_periods_year", "period_year BETWEEN 1 AND 9999");
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    expense_type = table.Column<short>(type: "smallint", nullable: false),
                    financial_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    actual_amount_currency = table.Column<short>(type: "smallint", nullable: false),
                    planned_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    planned_amount_currency = table.Column<short>(type: "smallint", nullable: false),
                    current_installment = table.Column<int>(type: "integer", nullable: true),
                    total_installments = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.id);
                    table.CheckConstraint("ck_expenses_actual_amount", "actual_amount >= 0");
                    table.CheckConstraint("ck_expenses_currency_consistency", "planned_amount_currency = actual_amount_currency");
                    table.CheckConstraint("ck_expenses_currency_values", "planned_amount_currency IN (1)\r\nAND actual_amount_currency IN (1)");
                    table.CheckConstraint("ck_expenses_expense_type", "expense_type IN (1, 2, 3)");
                    table.CheckConstraint("ck_expenses_installments", "(\r\n    expense_type = 2\r\n    AND current_installment IS NOT NULL\r\n    AND total_installments IS NOT NULL\r\n    AND current_installment > 0\r\n    AND total_installments > 0\r\n    AND current_installment <= total_installments\r\n)\r\nOR\r\n(\r\n    expense_type <> 2\r\n    AND current_installment IS NULL\r\n    AND total_installments IS NULL\r\n)");
                    table.CheckConstraint("ck_expenses_name", "btrim(name) <> ''");
                    table.CheckConstraint("ck_expenses_planned_amount", "planned_amount >= 0");
                    table.CheckConstraint("ck_expenses_status", "status IN (1, 2)");
                    table.ForeignKey(
                        name: "FK_expenses_financial_periods_financial_period_id",
                        column: x => x.financial_period_id,
                        principalTable: "financial_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    financial_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    actual_amount_currency = table.Column<short>(type: "smallint", nullable: false),
                    planned_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    planned_amount_currency = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomes", x => x.id);
                    table.CheckConstraint("ck_incomes_actual_amount", "actual_amount >= 0");
                    table.CheckConstraint("ck_incomes_currency_consistency", "planned_amount_currency = actual_amount_currency");
                    table.CheckConstraint("ck_incomes_currency_values", "planned_amount_currency IN (1)\r\nAND actual_amount_currency IN (1)");
                    table.CheckConstraint("ck_incomes_name", "btrim(name) <> ''");
                    table.CheckConstraint("ck_incomes_planned_amount", "planned_amount >= 0");
                    table.CheckConstraint("ck_incomes_status", "status IN (1, 2)");
                    table.ForeignKey(
                        name: "FK_incomes_financial_periods_financial_period_id",
                        column: x => x.financial_period_id,
                        principalTable: "financial_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "investments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    financial_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    actual_amount_currency = table.Column<short>(type: "smallint", nullable: false),
                    planned_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    planned_amount_currency = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investments", x => x.id);
                    table.CheckConstraint("ck_investments_actual_amount", "actual_amount >= 0");
                    table.CheckConstraint("ck_investments_currency_consistency", "planned_amount_currency = actual_amount_currency");
                    table.CheckConstraint("ck_investments_currency_values", "planned_amount_currency IN (1)\r\nAND actual_amount_currency IN (1)");
                    table.CheckConstraint("ck_investments_name", "btrim(name) <> ''");
                    table.CheckConstraint("ck_investments_planned_amount", "planned_amount >= 0");
                    table.CheckConstraint("ck_investments_status", "status IN (1, 2)");
                    table.ForeignKey(
                        name: "FK_investments_financial_periods_financial_period_id",
                        column: x => x.financial_period_id,
                        principalTable: "financial_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expense_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    transaction_date = table.Column<DateOnly>(type: "date", nullable: true),
                    planned_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    planned_amount_currency = table.Column<short>(type: "smallint", nullable: true),
                    expense_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    actual_amount_currency = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_details", x => x.id);
                    table.CheckConstraint("ck_expense_details_actual_amount", "actual_amount >= 0");
                    table.CheckConstraint("ck_expense_details_currency_consistency", "planned_amount_currency IS NULL\r\nOR planned_amount_currency = actual_amount_currency");
                    table.CheckConstraint("ck_expense_details_currency_values", "actual_amount_currency IN (1)\r\nAND\r\n(\r\n    planned_amount_currency IS NULL\r\n    OR planned_amount_currency IN (1)\r\n)");
                    table.CheckConstraint("ck_expense_details_description", "btrim(description) <> ''");
                    table.CheckConstraint("ck_expense_details_planned_amount", "planned_amount IS NULL OR planned_amount >= 0");
                    table.CheckConstraint("ck_expense_details_planned_amount_completeness", "(\r\n    planned_amount IS NULL\r\n    AND planned_amount_currency IS NULL\r\n)\r\nOR\r\n(\r\n    planned_amount IS NOT NULL\r\n    AND planned_amount_currency IS NOT NULL\r\n)");
                    table.ForeignKey(
                        name: "FK_expense_details_expenses_expense_id",
                        column: x => x.expense_id,
                        principalTable: "expenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_expense_details_expense_id",
                table: "expense_details",
                column: "expense_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_financial_period_id",
                table: "expenses",
                column: "financial_period_id");

            migrationBuilder.CreateIndex(
                name: "ux_financial_periods_year_month",
                table: "financial_periods",
                columns: new[] { "period_year", "period_month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_incomes_financial_period_id",
                table: "incomes",
                column: "financial_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_investments_financial_period_id",
                table: "investments",
                column: "financial_period_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense_details");

            migrationBuilder.DropTable(
                name: "incomes");

            migrationBuilder.DropTable(
                name: "investments");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "financial_periods");
        }
    }
}
