
using Microsoft.EntityFrameworkCore;
using Profile.Api.BackgroundServices;
using Profile.Api.Config;
using Profile.Api.DataAccess;
using Profile.Api.Services;

namespace Profile.Api
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
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDropInfoService, DropInfoService>();
            builder.Services.AddHostedService<ProfileBackgroundService>();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
