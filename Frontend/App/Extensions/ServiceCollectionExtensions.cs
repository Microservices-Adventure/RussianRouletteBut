using Frontend.App.Config;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Frontend.App.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });

        return services;
    }
}