namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Entities;
using Authorization.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class AppDbContextTests
{
    private readonly Mock<DbSet<User>> _mockUsers;
    private readonly Mock<DbSet<IdentityRole<long>>> _mockRoles;
    private readonly Mock<AppDbContext> _mockDbContext;

    public AppDbContextTests()
    {
        // Мокаем DbSet<User> и DbSet<IdentityRole<long>>
        _mockUsers = new Mock<DbSet<User>>();
        _mockRoles = new Mock<DbSet<IdentityRole<long>>>();

        // Мокаем AppDbContext
        var options = new DbContextOptions<AppDbContext>();
        _mockDbContext = new Mock<AppDbContext>(options);

        // Настраиваем моки для DbContext
        _mockDbContext.Setup(x => x.Users).Returns(_mockUsers.Object);
        _mockDbContext.Setup(x => x.Roles).Returns(_mockRoles.Object);
    }

    [Fact]
    public void AddUser_ShouldCallAddOnUsersDbSet()
    {
        // Arrange
        var user = new User { UserName = "test-user", Email = "test@example.com" };

        _mockUsers.Setup(x => x.Add(It.IsAny<User>()))
                  .Callback<User>(u =>
                  {
                      Assert.Equal("test-user", u.UserName);
                      Assert.Equal("test@example.com", u.Email);
                  });

        // Act
        _mockDbContext.Object.Users.Add(user);

        // Assert
        _mockUsers.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldCallSaveChangesAsyncOnDbContext()
    {
        // Arrange
        _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(1);

        // Act
        var result = await _mockDbContext.Object.SaveChangesAsync();

        // Assert
        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public void AddRole_ShouldCallAddOnRolesDbSet()
    {
        // Arrange
        var role = new IdentityRole<long> { Name = "User", NormalizedName = "USER", Id = 1 };

        _mockRoles.Setup(x => x.Add(It.IsAny<IdentityRole<long>>()))
                  .Callback<IdentityRole<long>>(r =>
                  {
                      Assert.Equal("User", r.Name);
                      Assert.Equal("USER", r.NormalizedName);
                      Assert.Equal(1, r.Id);
                  });

        // Act
        _mockDbContext.Object.Roles.Add(role);

        // Assert
        _mockRoles.Verify(x => x.Add(It.IsAny<IdentityRole<long>>()), Times.Once);
    }

    // Тестовый класс для вызова защищённого метода OnModelCreating
    private class TestAppDbContext : AppDbContext
    {
        public TestAppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public void CallOnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }
    }
}
