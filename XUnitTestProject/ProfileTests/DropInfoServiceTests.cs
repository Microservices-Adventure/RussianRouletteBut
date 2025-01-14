namespace XUnitTestProject.ProfileTests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Profile.Api.DataAccess;
using Profile.Api.DataAccess.Entity;
using Profile.Api.Models;
using Profile.Api.Services;

    public class DropInfoServiceTests
    {
        private readonly AppDbContext _context;
        private readonly DropInfoService _dropInfoService;

        public DropInfoServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);
            _dropInfoService = new DropInfoService(_context);
        }

        [Fact]
        public async Task AddUserProfileAsync_ShouldAddUser_WhenRequestIsValid()
        {
            // Arrange
            var request = new AddUserProfileRequest
            {
                Username = "testuser",
                Email = "test@example.com"
            };

            // Act
            var result = await _dropInfoService.AddUserProfileAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Username, result.Username);
            Assert.Equal(request.Email, result.Email);

            var userInDb = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == request.Username);
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task AddUserProfileAsync_ShouldThrowException_WhenRequestIsNull()
        {
            // Arrange
            AddUserProfileRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _dropInfoService.AddUserProfileAsync(request));
        }

        [Fact]
        public async Task AddUserProfileAsync_ShouldThrowException_WhenUsernameIsNullOrEmpty()
        {
            // Arrange
            var request = new AddUserProfileRequest
            {
                Username = "",
                Email = "test@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _dropInfoService.AddUserProfileAsync(request));
        }

        [Fact]
        public async Task AddUserProfileAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var existingUser = new UserProfile
            {
                Username = "existinguser",
                Email = "existing@example.com"
            };
            _context.UserProfiles.Add(existingUser);
            await _context.SaveChangesAsync();

            var request = new AddUserProfileRequest
            {
                Username = "existinguser",
                Email = "test@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _dropInfoService.AddUserProfileAsync(request));
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var existingUser = new UserProfile
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            _context.UserProfiles.Add(existingUser);
            await _context.SaveChangesAsync();

            var request = new GetUserProfileRequest
            {
                Username = "testuser"
            };

            // Act
            var result = await _dropInfoService.GetUserProfileAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUser.Username, result.Username);
            Assert.Equal(existingUser.Email, result.Email);
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserProfileRequest
            {
                Username = "nonexistentuser"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _dropInfoService.GetUserProfileAsync(request));
        }

        [Fact]
        public async Task AddDropInfoByUsernameAsync_ShouldAddDropInfo_WhenRequestIsValid()
        {
            // Arrange
            var user = new UserProfile
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();

            var request = new AddDropInfoByUsernameRequest
            {
                Username = "testuser",
                ServiceName = "testservice",
                Moment = DateTime.UtcNow
            };

            // Act
            var result = await _dropInfoService.AddDropInfoByUsernameAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.ServiceName, result.ServiceName);
            Assert.Equal(request.Moment, result.Moment);

            var dropInfoInDb = await _context.DropInfos.FirstOrDefaultAsync(d => d.Id == result.Id);
            Assert.NotNull(dropInfoInDb);
        }

        [Fact]
        public async Task AddDropInfoByUsernameAsync_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new AddDropInfoByUsernameRequest
            {
                Username = "newuser",
                ServiceName = "testservice",
                Moment = DateTime.UtcNow
            };

            // Act
            var result = await _dropInfoService.AddDropInfoByUsernameAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            var userInDb = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == request.Username);
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task AddDropInfoByUsernameAsync_ShouldThrowException_WhenRequestIsNull()
        {
            // Arrange
            AddDropInfoByUsernameRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _dropInfoService.AddDropInfoByUsernameAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task AddDropInfoByUsernameAsync_ShouldThrowException_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var request = new AddDropInfoByUsernameRequest
            {
                Username = "",
                ServiceName = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _dropInfoService.AddDropInfoByUsernameAsync(request, CancellationToken.None));
        }
    }