namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Config;
using Xunit;

    public class JwtSettingsTests
    {
        [Fact]
        public void JwtSettings_Properties_CanBeSetAndGet()
        {
            // Arrange
            var jwtSettings = new JwtSettings
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                SecretKey = "TestSecretKey"
            };

            // Act
            var issuer = jwtSettings.Issuer;
            var audience = jwtSettings.Audience;
            var secretKey = jwtSettings.SecretKey;

            // Assert
            Assert.Equal("TestIssuer", issuer);
            Assert.Equal("TestAudience", audience);
            Assert.Equal("TestSecretKey", secretKey);
        }

        [Fact]
        public void JwtSettings_Properties_AreNotNullAfterInitialization()
        {
            // Arrange
            var jwtSettings = new JwtSettings
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                SecretKey = "TestSecretKey"
            };

            // Act & Assert
            Assert.NotNull(jwtSettings.Issuer);
            Assert.NotNull(jwtSettings.Audience);
            Assert.NotNull(jwtSettings.SecretKey);
        }

        [Fact]
        public void JwtSettings_Properties_CanBeModified()
        {
            // Arrange
            var jwtSettings = new JwtSettings
            {
                Issuer = "InitialIssuer",
                Audience = "InitialAudience",
                SecretKey = "InitialSecretKey"
            };

            // Act
            jwtSettings.Issuer = "ModifiedIssuer";
            jwtSettings.Audience = "ModifiedAudience";
            jwtSettings.SecretKey = "ModifiedSecretKey";

            // Assert
            Assert.Equal("ModifiedIssuer", jwtSettings.Issuer);
            Assert.Equal("ModifiedAudience", jwtSettings.Audience);
            Assert.Equal("ModifiedSecretKey", jwtSettings.SecretKey);
        }
    }
