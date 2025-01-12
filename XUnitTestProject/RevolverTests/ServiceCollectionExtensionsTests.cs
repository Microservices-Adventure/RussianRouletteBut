using Microsoft.Extensions.Options;

namespace XUnitTestProject.RevolverTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Revolver.Api.Extensions;
using Revolver.Domain.Config;
using Xunit;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddConfigs_ShouldConfigureServicesParameters()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();

        configurationSection.Setup(x => x.Key).Returns(nameof(ServicesParameters));
        configuration.Setup(x => x.GetSection(nameof(ServicesParameters))).Returns(configurationSection.Object);

        // Act
        services.AddConfigs(configuration.Object);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<ServicesParameters>>();

        Assert.NotNull(options);
    }
}