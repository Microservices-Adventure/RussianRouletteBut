using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace XUnitTestProject.AuthorizationTests;

using Authorization.Api.Extensions;
using Authorization.Domain.Config;
using Authorization.Domain.Entities;

using Authorization.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAuthorizationServices_AddsAuthenticationAndAuthorization()
        {
            // Arrange
            var services = new Mock<IServiceCollection>();
            var jwtSettings = new JwtSettings
            {
                Issuer = "test-issuer",
                Audience = "test-audience",
                SecretKey = "test-secret-key-with-sufficient-length"
            };

            var authenticationBuilderMock = new Mock<AuthenticationBuilder>(services.Object);
            var jwtBearerOptions = new JwtBearerOptions();
            
            services
                .Setup(s => s.Add(It.Is<ServiceDescriptor>(sd =>
                    sd.ServiceType == typeof(IConfigureOptions<JwtBearerOptions>) &&
                    sd.ImplementationInstance is ConfigureNamedOptions<JwtBearerOptions>)))
                .Verifiable();

            // Act
            Authorization.Api.Extensions.ServiceCollectionExtensions.AddAuthorizationServices(services.Object, jwtSettings);

            // Assert
            services.Verify(); 
        }

        [Fact]
        public void AddDbServices_AddsDbContextAndIdentity()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "fake-connection-string";
            
            var userStoreMock = new Mock<IUserStore<User>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole<long>>>();
            
            var userManagerLoggerMock = new Mock<ILogger<UserManager<User>>>();
            var roleManagerLoggerMock = new Mock<ILogger<RoleManager<IdentityRole<long>>>>();
            
            services.AddSingleton(userStoreMock.Object);
            services.AddSingleton(roleStoreMock.Object);
            services.AddSingleton(userManagerLoggerMock.Object);
            services.AddSingleton(roleManagerLoggerMock.Object);

            // Act
            services.AddDbServices(connectionString);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<AppDbContext>();
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole<long>>>();

            Assert.NotNull(dbContext);
            Assert.NotNull(userManager);
            Assert.NotNull(roleManager);
        }

        [Fact]
        public void AddConfigs_AddsSwaggerAndConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "KafkaSettings:BootstrapServers", "localhost:9092" },
                    { "KafkaSettings:Topic", "test-topic" },
                    { "KafkaSettings:ConsumerGroupId", "test-group" },
                    { "JwtSettings:Issuer", "test-issuer" },
                    { "JwtSettings:Audience", "test-audience" },
                    { "JwtSettings:SecretKey", "test-secret-key" }
                })
                .Build();

            // Act
            services.AddConfigs(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var kafkaSettings = serviceProvider.GetService<IOptions<KafkaSettings>>()?.Value;
            var jwtSettings = serviceProvider.GetService<IOptions<JwtSettings>>()?.Value;

            Assert.NotNull(kafkaSettings);
            Assert.Equal("test-topic", kafkaSettings.Topic);
            Assert.Equal("test-group", kafkaSettings.ConsumerGroupId);

            Assert.NotNull(jwtSettings);
            Assert.Equal("test-issuer", jwtSettings.Issuer);
            Assert.Equal("test-audience", jwtSettings.Audience);
            Assert.Equal("test-secret-key", jwtSettings.SecretKey);
        }
    }
