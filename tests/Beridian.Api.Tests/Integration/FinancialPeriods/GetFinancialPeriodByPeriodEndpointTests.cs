using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Beridian.Application.FinancialPeriods.GetFinancialPeriod;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Beridian.Api.Tests.Integration.FinancialPeriods;

public sealed class GetFinancialPeriodByPeriodEndpointTests : IClassFixture<BeridianWebApplicationFactory>
{
    private readonly BeridianWebApplicationFactory _factory;

    public GetFinancialPeriodByPeriodEndpointTests(BeridianWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetByPeriod_WhenFinancialPeriodExists_ShouldReturnPeriod()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();
        var synchronizeRequest = new { Year = 2026, Month = 9 };
        var synchronizeResponse = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", synchronizeRequest);

        Assert.Equal(HttpStatusCode.OK, synchronizeResponse.StatusCode);

        // Act
        var response = await client.GetAsync("/api/v1/financial-periods/by-period?year=2026&month=9");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<GetFinancialPeriodResult>();

        Assert.NotNull(result);
        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.NotEqual(Guid.Empty, result.FinancialPeriodId);
    }

    [Fact]
    public async Task GetByPeriod_WhenFinancialPeriodDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/financial-periods/by-period?year=2026&month=9");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal("Financial period not found", problem.GetProperty("title").GetString());
        Assert.Equal(StatusCodes.Status404NotFound, problem.GetProperty("status").GetInt32());
        Assert.Equal(2026, problem.GetProperty("year").GetInt32());
        Assert.Equal(9, problem.GetProperty("month").GetInt32());
        Assert.False(await _factory.FinancialPeriodExistsAsync(2026, 9));
    }

    [Fact]
    public async Task GetByPeriod_WhenMonthIsInvalid_ShouldReturnValidationProblem()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/financial-periods/by-period?year=2026&month=13");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(StatusCodes.Status400BadRequest, problem.GetProperty("status").GetInt32());
        var errors = problem.GetProperty("errors");
        Assert.True(errors.TryGetProperty("month", out var monthErrors));
        Assert.Contains("Month must be between 1 and 12.", monthErrors.EnumerateArray().Select(error => error.GetString()));
        Assert.False(await _factory.FinancialPeriodExistsAsync(2026, 13));
    }    

}