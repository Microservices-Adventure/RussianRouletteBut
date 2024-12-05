using Frontend.App.Config;
using Frontend.App.Extensions;
using Frontend.App.Middlewares;
using Frontend.Features;
using Frontend.Features.Interfaces;

namespace Frontend.App;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();

                    options.ViewLocationFormats.Add("/Pages/{1}/Ui/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
                });
        
        services.Configure<KafkaSettings>(_configuration.GetSection(nameof(KafkaSettings)));
        services.AddExceptionHandler<AppExceptionHandler>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRevolverService, RevolverService>();

        services.AddApplicationAuthorization(_configuration);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseExceptionHandler(_ => { });

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

    }
}
