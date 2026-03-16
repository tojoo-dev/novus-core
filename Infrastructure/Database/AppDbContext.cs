using Microsoft.EntityFrameworkCore;
using Novus.Domain;

namespace Novus.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<TenantMembership> TenantMemberships { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Roles seeding
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Owner" },
            new Role { Id = 2, Name = "Admin" },
            new Role { Id = 3, Name = "Member" }
        );

        // TenantMembership relationships
        modelBuilder.Entity<TenantMembership>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<TenantMembership>()
            .HasOne(m => m.Company)
            .WithMany()
            .HasForeignKey(m => m.CompanyId);

        modelBuilder.Entity<TenantMembership>()
            .HasOne(m => m.Role)
            .WithMany()
            .HasForeignKey(m => m.RoleId);
    }
}
