# Idam.Libs.EF

Idam.Libs.EF is .Net Core (C#) for Entity Framework (EF) Utils.

## Features:

- Soft delete
- Timestamps (CreatedAt, UpdatedAt)

## Get started

run this command to install

```
Install-Package Idam.Libs.EF
```
or
```
dotnet tool install Idam.Libs.EF
```

## Usage
### Using Timestamps

1. Add following code in your context

```cs
using Idam.Libs.EF.Extensions;

public class MyDbContext : DbContext
{
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
}
```

2. Inherite ITimeStamps to your entity
```cs
using Idam.Libs.EF.Interfaces;

public class Foo : ITimeStamps
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
}
```

### Using SoftDelete

1. Add following code in your context

```cs git
using Idam.Libs.EF.Extensions;

public class MyDbContext : DbContext
{
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
        base.OnModelCreating(modelBuilder);

        var entityTypes = modelBuilder.Model.GetEntityTypes();
        modelBuilder.AddSoftDeleteFilter(entityTypes);
    }
}
```

2. Inherite ISoftDelete to your entity
```cs
using Idam.Libs.EF.Interfaces;

public class Foo : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public long? DeletedAt { get; set; }
}
```

### Using IGuidEntity
I create a interface to implement Id as Guid instead of int.
```cs
using Idam.Libs.EF.Interfaces;

public class Foo : IGuidEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
```