param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectName
)

Write-Host "Bootstrapping project: $ProjectName"

# Validate .NET SDK
$dotnetVersion = dotnet --version

if (-not $dotnetVersion.StartsWith("8.")) {
    Write-Error ".NET 8 SDK is required. Current version: $dotnetVersion"
    exit 1
}

# Create global.json if missing
if (-not (Test-Path "global.json")) {
    dotnet new globaljson --sdk-version $dotnetVersion
}

# Create solution
if (-not (Test-Path "$ProjectName.sln")) {
    dotnet new sln -n $ProjectName
}

# Create folders
New-Item -ItemType Directory -Force -Path "src" | Out-Null
New-Item -ItemType Directory -Force -Path "tests" | Out-Null

# Create projects
$projects = @(
    "Api",
    "Application",
    "Domain",
    "Infrastructure"
)

foreach ($project in $projects) {
    $projectPath = "src/$ProjectName.$project"

    if (-not (Test-Path $projectPath)) {
        if ($project -eq "Api") {
            dotnet new webapi -n "$ProjectName.$project" -o $projectPath
        } else {
            dotnet new classlib -n "$ProjectName.$project" -o $projectPath
        }

        dotnet sln "$ProjectName.sln" add "$projectPath/$ProjectName.$project.csproj"
    }
}

# Create test project
$testPath = "tests/$ProjectName.Tests"

if (-not (Test-Path $testPath)) {
    dotnet new xunit -n "$ProjectName.Tests" -o $testPath
    dotnet sln "$ProjectName.sln" add "$testPath/$ProjectName.Tests.csproj"
}

# Create gitignore
if (-not (Test-Path ".gitignore")) {
    dotnet new gitignore
}

Write-Host ""
Write-Host "Project initialized successfully."
Write-Host ""
Write-Host "Solution: $ProjectName.sln"
Write-Host "Source:   src/"
Write-Host "Tests:    tests/"
Write-Host ""
Write-Host "Next step:"
Write-Host "dotnet sln list"


Write-Host "Configuring project references..."
# Application -> Domain
dotnet add "src/$ProjectName.Application/$ProjectName.Application.csproj" reference `
    "src/$ProjectName.Domain/$ProjectName.Domain.csproj"

# Infrastructure -> Application + Domain
dotnet add "src/$ProjectName.Infrastructure/$ProjectName.Infrastructure.csproj" reference `
    "src/$ProjectName.Application/$ProjectName.Application.csproj"

dotnet add "src/$ProjectName.Infrastructure/$ProjectName.Infrastructure.csproj" reference `
    "src/$ProjectName.Domain/$ProjectName.Domain.csproj"

# API -> Application + Infrastructure
dotnet add "src/$ProjectName.Api/$ProjectName.Api.csproj" reference `
    "src/$ProjectName.Application/$ProjectName.Application.csproj"

dotnet add "src/$ProjectName.Api/$ProjectName.Api.csproj" reference `
    "src/$ProjectName.Infrastructure/$ProjectName.Infrastructure.csproj"

# Tests -> Application + Domain + Infrastructure
dotnet add "tests/$ProjectName.Tests/$ProjectName.Tests.csproj" reference `
    "src/$ProjectName.Application/$ProjectName.Application.csproj"

dotnet add "tests/$ProjectName.Tests/$ProjectName.Tests.csproj" reference `
    "src/$ProjectName.Domain/$ProjectName.Domain.csproj"

dotnet add "tests/$ProjectName.Tests/$ProjectName.Tests.csproj" reference `
    "src/$ProjectName.Infrastructure/$ProjectName.Infrastructure.csproj"

Write-Host ""
Write-Host "Building solution..."
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Bootstrap completed successfully." -ForegroundColor Green
}
else {
    Write-Host ""
    Write-Host "Build failed. Please review the errors." -ForegroundColor Red
}