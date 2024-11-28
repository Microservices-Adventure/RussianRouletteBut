namespace Authorization.Api;

public class Program
{
    public static void Main(string[] args)
    {
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
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                    .Build();
                    
                string certificatePath =
                    configuration["ASPNETCORE_Kestrel__Certificates__Default__:Path"]!;
                string certificatePassword =
                    configuration["ASPNETCORE_Kestrel__Certificates__Default__:Password"]!;
                
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(8083, listenOptions =>
                    {
                        listenOptions.UseHttps(certificatePath, certificatePassword);
                    });
                    serverOptions.ListenAnyIP(8082);
                });
                
                webBuilder.UseStartup<Startup>();
            });
}
