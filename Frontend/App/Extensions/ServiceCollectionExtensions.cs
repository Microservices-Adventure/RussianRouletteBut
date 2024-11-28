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
            .AddCookie(options => options.LoginPath = "/Account/Login");

        return services;
    }
}