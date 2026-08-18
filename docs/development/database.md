# Database Development Guide

## Technology

- PostgreSQL 16
- Entity Framework Core 8
- Npgsql

## Migration Naming Convention

Migration names must:

- Be written in English.
- Use PascalCase.
- Describe one concrete schema change.
- Begin with an explicit action when applicable.

Recommended prefixes:

- `Initial`
- `Add`
- `Create`
- `Alter`
- `Rename`
- `Remove`

Examples:

```text
InitialCreate
AddExpenseDetails
CreateFinancialPeriodIndexes
RenamePlannedValueToPlannedAmount
RemoveLegacyInvestmentColumn
```

Avoid vague names:
```text
Changes
Updates
FixDatabase
Migration2
NewFields
```

EF Core automatically prefixes the supplied name with a timestamp:
```text
20260814153042_AddExpenseDetails
```

The timestamp must not be manually modified.

Create a Migration
```text
dotnet ef migrations add AddExpenseDetails
```

Apply Migrations
```text
dotnet ef database update
```

Migration Review Rules
- Review Up() and Down() before applying the migration.
- Commit migration files and the model snapshot to Git.
- Do not manually rename generated migration files.
- Do not modify or remove migrations already applied to shared environments.
- Represent subsequent corrections through a new migration.