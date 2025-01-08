using Authorization.Api.Config;
using Authorization.Domain.Config;

namespace Authorization.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Wait {HealthSettings.CrashTime} seconds. Loading.");
        var startAt = HealthSettings.AppStartAt;
        Console.WriteLine($"Starting at {startAt}.");
        Thread.Sleep(TimeSpan.FromSeconds(HealthSettings.CrashTime));
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(8082);
                });
                
                webBuilder.UseStartup<Startup>();
            });
}
