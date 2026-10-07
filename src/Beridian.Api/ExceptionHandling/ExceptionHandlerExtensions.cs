using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Beridian.Api.ExceptionHandling;

public static class ExceptionHandlerExtensions
{
    public static IServiceCollection AddAllExceptionHandlers(
        this IServiceCollection services)
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        var handlerTypes = currentAssembly.GetTypes()
            .Where(type => 
                !type.IsAbstract && 
                !type.IsInterface && 
                type.IsAssignableTo(typeof(IExceptionHandler)) &&
                type != typeof(UnhandledExceptionHandler));

        foreach (var handlerType in handlerTypes)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton(typeof(IExceptionHandler), 
                handlerType));
        }

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton(
                typeof(IExceptionHandler),
                typeof(UnhandledExceptionHandler)));

        return services;
    }
}