using System.Text.Json;
using Beridian.Api.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Beridian.Api.Tests.ExceptionHandling;

public class UnhandledExceptionHandlerTests
{
    private readonly UnhandledExceptionHandler _sut;
    private readonly IServiceProvider _serviceProvider;

    public UnhandledExceptionHandlerTests()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddProblemDetails();

        _serviceProvider = services.BuildServiceProvider();

        var logger = _serviceProvider.GetRequiredService<ILogger<UnhandledExceptionHandler>>();
        _sut = new UnhandledExceptionHandler(logger);
    }

    [Fact]
    public async Task TryHandleAsync_WithUnhandledException_ShouldReturnTrueAndWriteProblemDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Sensitive internal exception message.");

        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        var responseStream = new MemoryStream();
        httpContext.Response.Body = responseStream;
        httpContext.Request.Path = "/api/v1/financial-periods";
        httpContext.Request.Method = HttpMethods.Post;

        // Act
        bool result = await _sut.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.True(result);

        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);

        Assert.Contains("application/problem+json", httpContext.Response.ContentType);

        responseStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(responseStream);
        string jsonResponse = await reader.ReadToEndAsync();

        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var root = jsonDocument.RootElement;

        Assert.Equal(StatusCodes.Status500InternalServerError, root.GetProperty("status").GetInt32());

        Assert.Equal("Internal Server Error", root.GetProperty("title").GetString());

        Assert.Equal("An unexpected error occurred while processing the request.", root.GetProperty("detail").GetString());

        Assert.Equal("/api/v1/financial-periods", root.GetProperty("instance").GetString());

        Assert.DoesNotContain(exception.Message, jsonResponse);
    }
}