
using ActionLog.Api.BackgroundServices;
using ActionLog.Api.Config;
using ActionLog.Api.DataAccess;
using ActionLog.Api.Services;
using Microsoft.EntityFrameworkCore;
using static ActionLog.Api.Services.LogServices;

namespace ActionLog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Wait {HealthSettings.CrashTime} seconds. Loading.");
            var startAt = HealthSettings.AppStartAt;
            Console.WriteLine($"Starting at {startAt}.");
            Thread.Sleep(TimeSpan.FromSeconds(HealthSettings.CrashTime));
            var builder = WebApplication.CreateBuilder(args);
            IConfiguration configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            string connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_Postgres_Connection") 
                                      ?? configuration.GetConnectionString("DefaultConnection")!;
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IHealthService, HealthService>();
            builder.Services.AddHostedService<LogBackgroundService>();
            builder.Services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }


}
