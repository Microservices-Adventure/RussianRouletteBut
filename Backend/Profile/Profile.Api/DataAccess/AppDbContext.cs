using Microsoft.EntityFrameworkCore;

namespace Profile.Api.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
