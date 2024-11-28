using Frontend.Entities.Account.Lib.Exceptions;
using Frontend.Entities.Account.Model;
using Microsoft.AspNetCore.Diagnostics;

namespace Frontend.App.Middlewares;

internal sealed class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is RequestLoginException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;
        }

        if (exception is ResponseLoginException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        if (exception is HttpRequestException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        }
        
        await httpContext.Response.WriteAsJsonAsync(exception, cancellationToken);
        
        return true;
    }
}