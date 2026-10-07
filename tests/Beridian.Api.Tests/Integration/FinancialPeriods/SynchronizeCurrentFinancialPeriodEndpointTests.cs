using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Beridian.Application.FinancialPeriods.SynchronizeCurrentFinancialPeriod;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Beridian.Api.Tests.Integration.FinancialPeriods;

public sealed class SynchronizeCurrentFinancialPeriodEndpointTests : IClassFixture<BeridianWebApplicationFactory>
{
    private readonly BeridianWebApplicationFactory _factory;

    public SynchronizeCurrentFinancialPeriodEndpointTests(BeridianWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Synchronize_WhenNoFinancialHistoryExists_ShouldCreateCurrentPeriod()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var request = new { Year = 2026, Month = 9 };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SynchronizeCurrentFinancialPeriodResult>();

        Assert.NotNull(result);
        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.True(result.WasCreated);
        Assert.NotEqual(Guid.Empty, result.FinancialPeriodId);
    }

    [Fact]
    public async Task Synchronize_WhenCurrentFinancialPeriodExists_ShouldReturnExistingPeriod()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var request = new { Year = 2026, Month = 9 };

        var firstResponse = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", request);

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        var firstResult = await firstResponse.Content.ReadFromJsonAsync<SynchronizeCurrentFinancialPeriodResult>();

        Assert.NotNull(firstResult);
        Assert.True(firstResult.WasCreated);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SynchronizeCurrentFinancialPeriodResult>();

        Assert.NotNull(result);

        Assert.Equal(firstResult.FinancialPeriodId, result.FinancialPeriodId);
        Assert.Equal(2026, result.Year);
        Assert.Equal(9, result.Month);
        Assert.False(result.WasCreated);
    }

    [Fact]
    public async Task Synchronize_WhenHistoryExistsButCurrentPeriodDoesNotExist_ShouldReturnConflict()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var augustRequest = new { Year = 2026, Month = 8 };

        var createResponse = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", augustRequest);

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var septemberRequest = new { Year = 2026, Month = 9 };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", septemberRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Current financial period not available", problem.GetProperty("title").GetString());
        Assert.Equal(StatusCodes.Status409Conflict, problem.GetProperty("status").GetInt32());
        Assert.Equal(2026, problem.GetProperty("year").GetInt32());
        Assert.Equal(9, problem.GetProperty("month").GetInt32());
        Assert.False(await _factory.FinancialPeriodExistsAsync(2026, 9));
    }


    [Fact]
    public async Task Synchronize_WhenMonthIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        using var client = _factory.CreateClient();
        var request = new { Year = 2026, Month = 13 };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/financial-periods/synchronize", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(await _factory.FinancialPeriodExistsAsync(2026, 13));
    }
}