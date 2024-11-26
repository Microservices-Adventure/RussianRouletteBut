namespace Frontend.App;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.Sources.Clear();
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("App/Config/appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"App/Config/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Console.WriteLine("Loading HTTPS certificate...");
                string? certificatePath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
                string? certificatePassword = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
                if (!string.IsNullOrEmpty(certificatePath) && !string.IsNullOrEmpty(certificatePassword))
                {
                    Console.WriteLine("HTTPS certificate load successfully");
                }
                else
                {
                    Console.WriteLine("HTTPS certificate was not loaded. Will use our-self certificate.");
                    
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("App/Config/appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"App/Config/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                        .Build();
                    
                    certificatePath =
                        configuration["ASPNETCORE_Kestrel__Certificates__Default__:Path"]!;
                    certificatePassword =
                        configuration["ASPNETCORE_Kestrel__Certificates__Default__:Password"]!;
                }
                
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(8081, listenOptions =>
                    {
                        listenOptions.UseHttps(certificatePath, certificatePassword);
                    });
                    serverOptions.ListenAnyIP(8080);
                });

                webBuilder.UseStartup<Startup>();
            });
}
