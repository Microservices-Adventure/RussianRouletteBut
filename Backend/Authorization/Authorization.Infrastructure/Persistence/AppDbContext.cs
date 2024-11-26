using Authorization.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole<long>> roles = new(){
                new IdentityRole<long>
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = 1
                },
                new IdentityRole<long>
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = 2
                },
            };
        modelBuilder.Entity<IdentityRole<long>>().HasData(roles);
    }
}
