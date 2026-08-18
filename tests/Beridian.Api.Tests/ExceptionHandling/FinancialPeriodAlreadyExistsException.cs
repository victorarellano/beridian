using System.Text.Json;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Xunit;
using Beridian.Api.ExceptionHandling;
using Microsoft.Extensions.DependencyInjection;

namespace Beridian.Api.Tests.ExceptionHandling;


public class FinancialPeriodAlreadyExistsException : Exception
{
    public int Year { get; }
    public int Month { get; }

    public FinancialPeriodAlreadyExistsException(int year, int month, string message) 
        : base(message)
    {
        Year = year;
        Month = month;
    }
}

internal sealed class FinancialPeriodAlreadyExistsExceptionHandler
    : ApiExceptionHandler<FinancialPeriodAlreadyExistsException>
{
    protected override ApiProblem CreateProblem(FinancialPeriodAlreadyExistsException exception)
    {
        return new ApiProblem(
            StatusCodes.Status409Conflict,
            "Financial period already exists",
            exception.Message,
            new Dictionary<string, object?>
            {
                ["year"] = exception.Year,
                ["month"] = exception.Month
            });
    }
}

public class FinancialPeriodAlreadyExistsExceptionHandlerTests
{
    private readonly FinancialPeriodAlreadyExistsExceptionHandler _sut;
    private readonly IServiceProvider _serviceProvider;

    public FinancialPeriodAlreadyExistsExceptionHandlerTests()
    {
        _sut = new FinancialPeriodAlreadyExistsExceptionHandler();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddProblemDetails();
        
        _serviceProvider = services.BuildServiceProvider();   
    }

    [Fact]
    public async Task TryHandleAsync_WithMatchingException_ShouldReturnTrueAndWriteProblemDetails()
    {
        //Arrange
        var exception = new FinancialPeriodAlreadyExistsException(2026, 8, "El periodo ya está cerrado.");
        
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };
        
        var responseStream = new MemoryStream();
        httpContext.Response.Body = responseStream;
        httpContext.Request.Path = "/api/v1/financial-periods";

        //Act
        bool result = await _sut.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.True(result, "El manejador debería retornar true al capturar FinancialPeriodAlreadyExistsException");
        
        Assert.Equal(StatusCodes.Status409Conflict, httpContext.Response.StatusCode);
        Assert.Contains("application/problem+json", httpContext.Response.ContentType);

        responseStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseStream);
        string jsonResponse = await reader.ReadToEndAsync();
        
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var root = jsonDocument.RootElement;

        // Validar propiedades estándar del RFC 7807
        Assert.Equal(StatusCodes.Status409Conflict, root.GetProperty("status").GetInt32());
        Assert.Equal("Financial period already exists", root.GetProperty("title").GetString());
        Assert.Equal("El periodo ya está cerrado.", root.GetProperty("detail").GetString());
        Assert.Equal("/api/v1/financial-periods", root.GetProperty("instance").GetString());

        // Validar extensiones personalizadas
        Assert.Equal(2026, root.GetProperty("year").GetInt32());
        Assert.Equal(8, root.GetProperty("month").GetInt32());
    }

    [Fact]
    public async Task TryHandleAsync_WithDifferentException_ShouldReturnFalseAndNotModifyResponse()
    {
        //Arrange
        var unrelatedException = new InvalidOperationException("Operación inválida del sistema.");

        var httpContext = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider
        };

        var responseStream = new MemoryStream();
        httpContext.Response.Body = responseStream;

        //Act
        bool result = await _sut.TryHandleAsync(httpContext, unrelatedException, CancellationToken.None);

        //Assert
        Assert.False(result, "porque este manejador está especializado únicamente en FinancialPeriodAlreadyExistsException");

        Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
        Assert.Equal(0, responseStream.Length);
    }
}

