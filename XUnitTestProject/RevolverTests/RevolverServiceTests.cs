namespace XUnitTestProject.RevolverTests;

using System;
using System.Collections.Generic;
using Revolver.Domain.Config;
using Revolver.Domain.Services;
using Xunit;

public class RevolverServiceTests
{
    [Fact]
    public void Roll_ReturnsServiceInfoFromTheList()
    {
        // Arrange
        var services = new List<ServiceInfo>
        {
            new ServiceInfo { Host = "http://service1.com" },
            new ServiceInfo { Host = "http://service2.com" },
            new ServiceInfo { Host = "http://service3.com" }
        };

        var service = new RevolverService();

        // Act
        var result = service.Roll(services);

        // Assert
        Assert.Contains(result, services);
    }

    [Fact]
    public void Roll_NotReturnServiceInfoNotFromTheList()
    {
        // Arrange
        var services = new List<ServiceInfo>
        {
            new ServiceInfo { Host = "http://service1.com" },
            new ServiceInfo { Host = "http://service2.com" },
            new ServiceInfo { Host = "http://service3.com" }
        };

        var service = new RevolverService();

        // Act
        var result = service.Roll(services);

        // Assert
        Assert.DoesNotContain(new ServiceInfo { Host = "http://service4.com" }, services);
    }

    [Fact]
    public void Roll_ProcessesAnEmptyList()
    {
        // Arrange
        var services = new List<ServiceInfo>();

        var service = new RevolverService();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => service.Roll(services));
    }
}