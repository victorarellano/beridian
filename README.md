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

dotnet new xunit -n Beridian.Tests

cd ..

dotnet sln Beridian.sln add src/Beridian.Api/Beridian.Api.csproj
dotnet sln Beridian.sln add src/Beridian.Application/Beridian.Application.csproj
dotnet sln Beridian.sln add src/Beridian.Domain/Beridian.Domain.csproj
dotnet sln Beridian.sln add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet sln Beridian.sln add tests/Beridian.Tests/Beridian.Tests.csproj

dotnet add src/Beridian.Application/Beridian.Application.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add src/Beridian.Infrastructure/Beridian.Infrastructure.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add src/Beridian.Api/Beridian.Api.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add src/Beridian.Api/Beridian.Api.csproj reference src/Beridian.Infrastructure/Beridian.Infrastructure.csproj
dotnet add tests/Beridian.Tests/Beridian.Tests.csproj reference src/Beridian.Application/Beridian.Application.csproj
dotnet add tests/Beridian.Tests/Beridian.Tests.csproj reference src/Beridian.Domain/Beridian.Domain.csproj
dotnet add tests/Beridian.Tests/Beridian.Tests.csproj reference src/Beridian.Infrastructure/Beridian.Infrastructure.csproj

dotnet build

```

At this stage, the solution follows the **Clean Architecture** structure.

The projects will be added to the solution and configured during the next development steps.

## References Packages

```text
dotnet add src/Beridian.Application package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add src/Beridian.Infrastructure package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add src/Beridian.Infrastructure package Microsoft.Extensions.Configuration.Abstractions

dotnet add src/Beridian.Api reference src/Beridian.Application
dotnet add src/Beridian.Api reference src/Beridian.Infrastructure
```

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

🚧 Under Development