using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Media> Media { get; set; }
    public DbSet<Season> Season { get; set; }
    public DbSet<Episode> Episode { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}