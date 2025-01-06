using Microsoft.EntityFrameworkCore;
using Profile.Api.DataAccess.Entity;

namespace Profile.Api.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<DropInfo> DropInfos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
