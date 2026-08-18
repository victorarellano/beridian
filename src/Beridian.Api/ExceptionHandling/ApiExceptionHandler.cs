using Microsoft.AspNetCore.Diagnostics;

namespace Beridian.Api.ExceptionHandling;

/// <summary>
/// Abstract base class for centralized exception handling.
/// Transform domain exceptions into standarized response under the RFC 7807 standard. 
/// </summary>
internal abstract class ApiExceptionHandler<TException> 
    : IExceptionHandler where TException : Exception
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not TException handledException)
        {
            return false;
        }

        var problem = CreateProblem(handledException);

        await Results.Problem(
                statusCode: problem.Status,
                title: problem.Title,
                detail: problem.Detail,
                instance: httpContext.Request.Path,
                extensions: problem.Extensions)
            .ExecuteAsync(httpContext);

        return true;
    }

    protected abstract ApiProblem CreateProblem(
        TException exception);

    protected sealed record ApiProblem(
        int Status,
        string Title,
        string Detail,
        IDictionary<string, object?>? Extensions = null);
}