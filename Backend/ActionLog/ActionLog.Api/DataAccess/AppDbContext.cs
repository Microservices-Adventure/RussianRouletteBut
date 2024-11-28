using Microsoft.EntityFrameworkCore;
using ActionLog.Api.DataAccess.Entity;

namespace ActionLog.Api.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<ALog> ActionLogs { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
