using Frontend.App;
using Frontend.Features.Services.Interfaces;

namespace XUnitTestProject.FrontendTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class StartupTests
{
    [Fact]
    public void ConfigureServices_RegistersDependenciesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var startup = new Startup(configuration);

        // Act
        startup.ConfigureServices(services);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IAccountService>());
        Assert.NotNull(serviceProvider.GetService<IRevolverService>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IProfileService>());
    }
}