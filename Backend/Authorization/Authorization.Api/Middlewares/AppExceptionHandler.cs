using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace Authorization.Api.Middlewares;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

        if (exception is UnauthorizedAccessException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        
        await httpContext.Response.WriteAsJsonAsync(exception, cancellationToken);
        
        return true;
    }
}