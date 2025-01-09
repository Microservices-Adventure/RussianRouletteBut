using Revolver.Domain.Config;

namespace Revolver.Api;

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

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}