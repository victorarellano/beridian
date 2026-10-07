using System.Text.Json;
using Beridian.Api.ExceptionHandling.FinancialPeriods;
using Beridian.Application.Exceptions;
using Beridian.Application.FinancialPeriods.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Beridian.Api.Tests.ExceptionHandling;

public class FinancialPeriodByPeriodNotFoundExceptionHandlerTests
{
    private readonly FinancialPeriodByPeriodNotFoundExceptionHandler _sut;
    private readonly IServiceProvider _serviceProvider;

    public FinancialPeriodByPeriodNotFoundExceptionHandlerTests()
    {
        _sut = new FinancialPeriodByPeriodNotFoundExceptionHandler();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddProblemDetails();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task TryHandleAsync_WithMatchingException_ShouldReturnTrueAndWriteProblemDetails()
    {
        // Arrange
        var exception = new FinancialPeriodByPeriodNotFoundException(2026, 10);

        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        var responseStream = new MemoryStream();
        httpContext.Response.Body = responseStream;
        httpContext.Request.Path = "/api/v1/financial-periods/by-period";
        httpContext.Request.QueryString = new QueryString("?year=2026&month=10");

        // Act
        bool result = await _sut.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        Assert.Contains("application/problem+json", httpContext.Response.ContentType);

        responseStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(responseStream);
        string jsonResponse = await reader.ReadToEndAsync();

        using var jsonDocument = JsonDocument.Parse(jsonResponse);
        var root = jsonDocument.RootElement;

        Assert.Equal(StatusCodes.Status404NotFound, root.GetProperty("status").GetInt32());
        Assert.Equal("Financial period not found", root.GetProperty("title").GetString());
        Assert.Equal(exception.Message, root.GetProperty("detail").GetString());
        Assert.Equal("/api/v1/financial-periods/by-period", root.GetProperty("instance").GetString());
        Assert.Equal(2026, root.GetProperty("year").GetInt32());
        Assert.Equal(10, root.GetProperty("month").GetInt32());
    }

    [Fact]
    public async Task TryHandleAsync_WithDifferentException_ShouldReturnFalseAndNotModifyResponse()
    {
        // Arrange
        var unrelatedException = new InvalidOperationException("Unexpected exception.");

        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        var responseStream = new MemoryStream();
        httpContext.Response.Body = responseStream;

        // Act
        bool result = await _sut.TryHandleAsync(httpContext, unrelatedException, CancellationToken.None);

        // Assert
        Assert.False(result);
        Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
        Assert.Equal(0, responseStream.Length);
    }
}