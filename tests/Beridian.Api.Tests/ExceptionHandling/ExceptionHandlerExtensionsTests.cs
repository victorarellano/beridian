using Beridian.Api.ExceptionHandling;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Beridian.Api.Tests.ExceptionHandling;

public class ExceptionHandlerExtensionsTests
{
    [Fact]
    public void AddAllExceptionHandlers_ShouldRegisterUnhandledExceptionHandlerLast()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAllExceptionHandlers();

        // Assert
        var handlers = services
            .Where(service => service.ServiceType == typeof(IExceptionHandler))
            .ToList();

        Assert.NotEmpty(handlers);

        Assert.Equal(typeof(UnhandledExceptionHandler), handlers.Last().ImplementationType);
    }
}