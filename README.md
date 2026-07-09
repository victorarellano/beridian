# Debt Manager

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

## Prerequisites

Before running the project, ensure the following tools are installed:

- .NET SDK 8 LTS
- Git
- Docker Desktop
- Visual Studio Code (recommended)
- Visual Studio 2022 (optional)

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
dotnet new sln -n DebtManager
```

Create the project folders:

```text
src/
tests/
```

Create the projects:

```bash
cd src

dotnet new webapi -n DebtManager.Api
dotnet new classlib -n DebtManager.Application
dotnet new classlib -n DebtManager.Domain
dotnet new classlib -n DebtManager.Infrastructure

cd ..

cd tests

dotnet new xunit -n DebtManager.Tests

cd ..

dotnet sln DebtManager.sln add src/DebtManager.Api/DebtManager.Api.csproj

dotnet sln DebtManager.sln add src/DebtManager.Application/DebtManager.Application.csproj

dotnet sln DebtManager.sln add src/DebtManager.Domain/DebtManager.Domain.csproj

dotnet sln DebtManager.sln add src/DebtManager.Infrastructure/DebtManager.Infrastructure.csproj

dotnet sln DebtManager.sln add tests/DebtManager.Tests/DebtManager.Tests.csproj

```

At this stage, the solution follows the **Clean Architecture** structure.

The projects will be added to the solution and configured during the next development steps.

---

## Configure Git

Generate the standard .NET gitignore file:

```bash
dotnet new gitignore
```
---

## Alternative
```bash
.\scripts\bootstrap-project.ps1 -ProjectName DebtManager
dotnet sln list
```

## Project Status

🚧 Under Development