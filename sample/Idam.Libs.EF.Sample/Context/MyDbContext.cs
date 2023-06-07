using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Sample.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Sample.Context;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Foo> Foos { get; set; }
    public DbSet<Boo> Boos { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.Entries().AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.Entries().AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        modelBuilder.AddSoftDeleteFilter(entityTypes);

        base.OnModelCreating(modelBuilder);
    }
}
