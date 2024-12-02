using Revolver.Api.Extensions;
using Revolver.Domain.Config;
using Revolver.Domain.Services;
using Revolver.Domain.Services.Interfaces;

namespace Revolver.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBaseServices();
        services.AddSingleton<IRevolverService, RevolverService>();
        services.Configure<ServicesParameters>(_configuration.GetSection(nameof(ServicesParameters)));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}