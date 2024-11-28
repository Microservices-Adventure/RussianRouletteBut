using Authorization.Api.BackgroundServices;
using Authorization.Api.Config;
using Authorization.Api.Extensions;
using Authorization.Domain.Config;
using Authorization.Domain.Entities;
using Authorization.Domain.Services;
using Authorization.Domain.Services.Interfaces;
using Authorization.Domain.Validators;
using Authorization.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        services.AddConfigs(_configuration);
        services.AddAuthorizationServices(_configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!);

        var connectionString = _configuration["ConnectionStrings:DefaultConnection"]!;
        services.AddDbServices(connectionString);
        services.AddApplicationServices();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }

        app.UseExceptionHandler(_ => { });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
            endpoints.MapControllers()
        );

    }
}