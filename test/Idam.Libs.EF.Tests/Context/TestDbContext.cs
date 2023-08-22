using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Context;
public class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Foo> Foos { get; set; }
    public DbSet<Boo> Boos { get; set; }
    public DbSet<Aoo> Aoos { get; set; }
    public DbSet<Doo> Doos { get; set; }
    public DbSet<Cdoo> Cdoos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Idam.Libs.EF.Tests");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddSoftDeleteFilter();

        base.OnModelCreating(modelBuilder);
    }
}
