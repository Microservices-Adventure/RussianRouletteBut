using Authorization.Api.Config;
using Authorization.Api.Extensions;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace Authorization.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();

        services.AddAuthorizationServices(_configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
            endpoints.MapControllers()
        );

    }
}