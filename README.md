# Beridian

A modern personal finance management platform built with .NET 8, PostgreSQL and cloud-native technologies.

---

## Vision

Build an intelligent personal finance platform that evolves from a traditional expense tracker into an AI-powered financial assistant.

---

## Technology Stack

- .NET 8 LTS
- ASP.NET Core
- PostgreSQL
- Entity Framework Core
- Docker
- Kubernetes (future)
- OpenTelemetry (future)

---

## Architecture

Beridian follows Clean Architecture and separates the solution into four main projects:

```text
src/
├── Beridian.Api             HTTP endpoints, API versioning and exception handling
├── Beridian.Application     Application use cases and persistence abstractions
├── Beridian.Domain          Aggregates, entities, value objects and domain rules
└── Beridian.Infrastructure  Entity Framework Core and PostgreSQL persistence
```

The main domain aggregate is `FinancialPeriod`. It protects the consistency of incomes, expenses, investments, balances and the financial-period lifecycle.

Dependencies point toward the domain:

```text
Api -> Application
Api -> Infrastructure
Infrastructure -> Application
Infrastructure -> Domain
Application -> Domain
```

---

## API

The initial API is implemented with ASP.NET Core Minimal APIs and URL-based API versioning. Version 1 uses the following base route:

```text
/api/v1/financial-periods
```

### Financial Periods

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/v1/financial-periods` | Create a financial period. |
| `GET` | `/api/v1/financial-periods/{financialPeriodId}` | Get a financial period and its current composition. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/close` | Close a financial period. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/next` | Generate the next financial period. |

### Incomes

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/incomes` | Add an income to an open financial period. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/incomes/{incomeId}/entry` | Enter the actual income amount. |

### Expenses

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/recurring` | Add a recurring expense. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/fixed-term` | Add a fixed-term expense. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/discretionary` | Add a discretionary expense. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/details` | Add a detail to an expense. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry` | Enter an expense using an explicit actual amount. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/expenses/{expenseId}/entry-from-details` | Enter an expense using the sum of its details. |

### Investments

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/investments` | Add an investment to an open financial period. |
| `POST` | `/api/v1/financial-periods/{financialPeriodId}/investments/{investmentId}/confirmation` | Confirm an investment using its actual amount. |

### OpenAPI

When the API runs in the Development environment, Swagger UI is available at:

```text
/swagger
```

Application and domain exceptions are translated into RFC 7807 Problem Details responses. The API currently uses `400 Bad Request`, `404 Not Found` and `409 Conflict` to represent validation failures, missing resources and business-rule conflicts.

---

## Prerequisites

Before running the project, ensure the following tools are installed:

- .NET SDK 8 LTS
- Git
- Docker Desktop
- Visual Studio Code (recommended)
- Visual Studio 2022 (optional)

---

## Run the Project Locally

Follow these steps to clone and run the existing Beridian repository.

### Clone the Repository

```bash
git clone https://github.com/victorarellano/beridian.git
cd beridian
```

### Restore Dependencies

```bash
dotnet restore
dotnet build
```

### Configure PostgreSQL

Create a `.env` file in the solution root using `.env.example` as a template.

PowerShell:

```powershell
Copy-Item .env.example .env
```

Bash:

```bash
cp .env.example .env
```

Update the local values:

```dotenv
POSTGRES_DB=beridian
POSTGRES_USER=beridian
POSTGRES_PASSWORD=replace_with_local_password
```

The `.env` file contains local credentials and must not be committed to Git.

### Start PostgreSQL

```bash
docker compose up -d
docker compose ps
```

Wait until the `beridian-postgres` container reports a `healthy` status.

### Configure the Application Connection String

The password must match `POSTGRES_PASSWORD` from the `.env` file.

```powershell
dotnet user-secrets set "ConnectionStrings:Database" "Host=localhost;Port=5432;Database=beridian;Username=beridian;Password=replace_with_local_password" --project src/Beridian.Api/Beridian.Api.csproj
```

### Apply Database Migrations

```powershell
dotnet ef database update --project src/Beridian.Infrastructure/Beridian.Infrastructure.csproj --startup-project src/Beridian.Api/Beridian.Api.csproj --context BeridianDbContext
```

### Run the API

```bash
dotnet run --project src/Beridian.Api/Beridian.Api.csproj
```

Use the URL displayed in the terminal to access the running API.

### Run the Tests

Run all automated tests from the solution root:

```bash
dotnet test
```

The solution contains unit tests for the Domain and Application layers and integration tests for Infrastructure persistence using PostgreSQL Testcontainers. The initial API endpoints were also verified manually through Swagger during Sprint 2.

### Stop the Local Database

```bash
docker compose down
```

This stops the PostgreSQL container without deleting the persisted data stored in the named Docker volume.

---

## Verify Installed SDKs

Run:

```bash
dotnet --list-sdks
```

Expected output should include a .NET 8 SDK, for example:

```text
8.0.413
```

---

## Install .NET 8 SDK

If .NET 8 is not installed, download it from:

https://dotnet.microsoft.com/download/dotnet/8.0

---

## Configure the Project SDK

This repository uses a `global.json` file to lock the SDK version.

To create it:

```bash
dotnet new globaljson --sdk-version 8.0.413
```

Verify:

```bash
dotnet --version
```

Expected output:

```text
8.0.413
```

---

## Initialize the Solution

Create the solution file:

```bash
dotnet new sln -n Beridian
```

Create the project folders:

```text
src/
tests/
```

Create the projects:

```bash
cd src

dotnet new webapi -n Beridian.Api
dotnet new classlib -n Beridian.Application
dotnet new classlib -n Beridian.Domain
dotnet new classlib -n Beridian.Infrastructure

cd ..

cd tests

dotnet new xunit -n Beridian.Domain.Tests
dotnet new xunit -n Beridian.Application.Tests
dotnet new xunit -n Beridian.Infrastructure.Tests
dotnet new xunit -n Beridian.Api.Tests
cd ..

dotnet sln Beridian.sln add src/Beridian.Api/Beridian.Api.csproj
dotnet sln Beridian.sln add src/Beridian.Application/Beridian.Application.csproj
dotnet sln Beridian.sln add src/Beridian.Domain/Beridian.Domain.csproj
dotnet sln Beridian.sln add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet sln Beridian.sln add tests/Beridian.Domain.Tests/Beridian.Domain.Tests.csproj
dotnet sln Beridian.sln add tests/Beridian.Application.Tests/Beridian.Application.Tests.csproj
dotnet sln Beridian.sln add tests/Beridian.Api.Tests/Beridian.Api.Tests.csproj
dotnet sln Beridian.sln add tests/Beridian.Infrastructure.Tests/Beridian.Infrastructure.Tests.csproj

dotnet add src/Beridian.Application/Beridian.Application.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add src/Beridian.Api/Beridian.Api.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add src/Beridian.Api/Beridian.Api.csproj reference src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet add tests/Beridian.Domain.Tests/Beridian.Domain.Tests.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add tests/Beridian.Domain.Tests/Beridian.Domain.Tests.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add tests/Beridian.Domain.Tests/Beridian.Domain.Tests.csproj reference src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet add tests/Beridian.Infrastructure.Tests/Beridian.Infrastructure.Tests.csproj reference src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet add tests/Beridian.Infrastructure.Tests/Beridian.Infrastructure.Tests.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add tests/Beridian.Infrastructure.Tests/Beridian.Infrastructure.Tests.csproj package Testcontainers.PostgreSql
dotnet add tests/Beridian.Api.Tests/Beridian.Api.Tests.csproj reference src/Beridian.Api/Beridian.Api.csproj
dotnet add tests/Beridian.Api.Tests/Beridian.Api.Tests.csproj package FluentAssertions

dotnet build

```

At this stage, the solution follows the **Clean Architecture** structure.

The projects will be added to the solution and configured during the next development steps.

---

## References Packages

```text
dotnet add src/Beridian.Application package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add src/Beridian.Infrastructure package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add src/Beridian.Infrastructure package Microsoft.Extensions.Configuration.Abstractions

dotnet add src/Beridian.Api reference src/Beridian.Application
dotnet add src/Beridian.Api reference src/Beridian.Infrastructure
dotnet add src/Beridian.Api/Beridian.Api.csproj package Asp.Versioning.Http --version 8.1.0
dotnet add src/Beridian.Api/Beridian.Api.csproj package Asp.Versioning.Mvc.ApiExplorer --version 8.1.0
dotnet add src/Beridian.Api/Beridian.Api.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.8
```

---

## Configure Infrastructure Persistence Initial
Add Entity Framework Core and the PostgreSQL provider:

```bash
cd src/Beridian.Infrastructure

dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.8

```

### Install the Entity Framework Core
```bash
dotnet tool install --global dotnet-ef --version 8.0.8
dotnet tool update --global dotnet-ef --version 8.0.8

dotnet ef --version

cd ../..

dotnet restore
dotnet build
```

For migration commands and conventions, see [Database Development Guide](docs/development/database.md).

### Configure the Local Database Secret

Beridian reads the PostgreSQL connection string from the `ConnectionStrings:Database` configuration key.

Initialize .NET User Secrets for the API project:

```bash
dotnet user-secrets init \
  --project src/Beridian.Api/Beridian.Api.csproj
```

Configure the local database connection, replacing the placeholder values with the local PostgreSQL credentials:

```bash
dotnet user-secrets set \
  "ConnectionStrings:Database" \
  "Host=localhost;Port=5432;Database=beridian;Username=beridian;Password=YOUR_PASSWORD" \
  --project src/Beridian.Api/Beridian.Api.csproj
```

Verify that the configuration was registered:

```bash
dotnet user-secrets list \
  --project src/Beridian.Api/Beridian.Api.csproj
```

User Secrets are intended only for local development. The secret values are stored outside the repository and must not be committed to Git.

Deployment environments must provide the connection string through their secure configuration mechanism or through the following environment variable:

```text
ConnectionStrings__Database
```
---

### Create the Initial Database Migration

Run the following command from the solution root:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Beridian.Infrastructure/Beridian.Infrastructure.csproj \
  --startup-project src/Beridian.Api/Beridian.Api.csproj \
  --context BeridianDbContext \
  --output-dir Persistence/Migrations
```

For PowerShell, the command can be executed on a single line:

```powershell
dotnet ef migrations add InitialCreate --project src/Beridian.Infrastructure/Beridian.Infrastructure.csproj --startup-project src/Beridian.Api/Beridian.Api.csproj --context BeridianDbContext --output-dir Persistence/Migrations
```

The options indicate:

* `--project`: the project containing `BeridianDbContext` and the migrations.
* `--startup-project`: the executable project that supplies application configuration and dependency registration.
* `--context`: the EF Core context used to generate the model.
* `--output-dir`: the migration directory relative to the Infrastructure project.

The command generates:

```text
src/Beridian.Infrastructure/
└── Persistence/
    └── Migrations/
        ├── <timestamp>_InitialCreate.cs
        ├── <timestamp>_InitialCreate.Designer.cs
        └── BeridianDbContextModelSnapshot.cs
```

Creating a migration validates the EF Core model but does not apply changes to PostgreSQL.

Review the generated migration before committing it or applying it to a database. In particular, verify:

* Table and column names.
* Primary and foreign keys.
* Nullability.
* Numeric precision.
* Discriminator values.
* Check constraints.
* Cascade deletion rules.
* Indexes.

For revert generate migration, execute command:
```powershell
dotnet ef migrations remove --project src/Beridian.Infrastructure/Beridian.Infrastructure.csproj --startup-project src/Beridian.Api/Beridian.Api.csproj --context BeridianDbContext
```

Migration files and the model snapshot must be committed to Git.

---

### Run PostgreSQL Locally with Docker Compose

Beridian uses PostgreSQL 16 for local persistence. The database runs in a Docker container and stores its data in a named volume.

#### Configure Docker Compose

Create a `compose.yml` file in the solution root:

```yaml
services:
  postgres:
    image: postgres:16
    container_name: beridian-postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: ${POSTGRES_DB}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - beridian-postgres-data:/var/lib/postgresql/data
    healthcheck:
      test:
        - CMD-SHELL
        - pg_isready -U ${POSTGRES_USER} -d ${POSTGRES_DB}
      interval: 5s
      timeout: 5s
      retries: 10

volumes:
  beridian-postgres-data:
```

#### Configure Local Environment Variables

Create a `.env` file in the solution root:

```dotenv
POSTGRES_DB=beridian
POSTGRES_USER=beridian
POSTGRES_PASSWORD=replace_with_local_password
```

The `.env` file contains local credentials and must not be committed. Ensure `.gitignore` contains:

```gitignore
.env
```

Create a version-controlled `.env.example` file containing only placeholder values:

```dotenv
POSTGRES_DB=beridian
POSTGRES_USER=beridian
POSTGRES_PASSWORD=replace_with_local_password
```

#### Configure the Application Connection String

The password in the connection string must match `POSTGRES_PASSWORD` from the local `.env` file.

Store the connection string using .NET User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:Database" "Host=localhost;Port=5432;Database=beridian;Username=beridian;Password=replace_with_local_password" --project src/Beridian.Api/Beridian.Api.csproj
```

User Secrets are used only for local development and are not committed to the repository.

#### Start PostgreSQL

From the solution root, run:

```powershell
docker compose up -d
```

Verify the container status:

```powershell
docker compose ps
```

The `beridian-postgres` container should report a `healthy` status before database operations are executed.

To inspect its logs:

```powershell
docker compose logs postgres
```

#### Stop PostgreSQL

Stop the container without deleting its persisted data:

```powershell
docker compose down
```

The `beridian-postgres-data` volume remains available and will be reused the next time PostgreSQL starts.


---

## Configure Git

Generate the standard .NET gitignore file:

```bash
dotnet new gitignore
```
Initialize the local Git repository:

```bash
git init
```

Rename the default branch:

```bash
git branch -M main
```

Connect the local repository to GitHub:

```bash
git remote add origin <repository-url>
```

Verify the remote configuration:

```bash
git remote -v
```

---

## First Commit

Stage all project files:

```bash
git add .
```

Create the initial commit:

```bash
git commit -m "Initialize project structure"
```

Publish the repository to GitHub:

```bash
git push -u origin main
```

The `-u` option configures the upstream branch so future pushes only require:

```bash
git push
```

and updates can be retrieved with:

```bash
git pull
```

---

## Project Bootstrap

The project includes a PowerShell bootstrap script that automates the initial project setup.

Run:

```powershell
.\scripts\bootstrap-project.ps1 -ProjectName Beridian
```

The script performs the following tasks:

- Validates that .NET 8 SDK is installed.
- Creates the `global.json` file (if missing).
- Creates the solution.
- Creates the `src` and `tests` folders.
- Generates the Clean Architecture projects.
- Adds all projects to the solution.
- Creates the official .NET `.gitignore` file.
- Displays a summary of the generated structure.

This script is intended to automate the initial project setup and can be reused by changing only the project name.

---

## Project Status

**Sprint 2 — MVP Backend: Completed**

The current backend milestone includes:

- Clean Architecture solution structure.
- Financial-period domain model and invariants.
- Application use cases for financial periods, incomes, expenses and investments.
- PostgreSQL persistence with Entity Framework Core migrations.
- Infrastructure integration tests using PostgreSQL Testcontainers.
- Versioned Minimal API endpoints with Swagger documentation.
- Centralized Problem Details exception handling.
- Manual verification of the initial API endpoints.

Beridian remains under active development. Automated HTTP integration tests and update/delete operations are candidates for a future iteration and are not part of the current MVP backend scope.
