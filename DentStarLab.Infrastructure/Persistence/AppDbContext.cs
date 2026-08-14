using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<WorkType> WorkTypes => Set<WorkType>();
    public DbSet<Work> Works => Set<Work>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
