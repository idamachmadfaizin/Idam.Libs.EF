# Idam.Libs.EF
[Idam.Libs.EF](https://github.com/idamachmadfaizin/Idam.Libs.EF) is .Net Core (C#) for Entity Framework (EF) Utils.

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

## Features:
- Soft delete (CreatedAt).
- Timestamps (CreatedAt, UpdatedAt).

Both features support DateTime and [Unix Time Milliseconds](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimemilliseconds?view=net-7.0) format.

Example of Unix Time Milliseconds: [currentmillis](https://currentmillis.com)

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

1. Add `AddTimestamps()` in your context.

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

2. Implement a Interface (`ITimeStamps` or `ITimeStampsUnix`) to your entity.

```cs
using Idam.Libs.EF.Interfaces;

// Using DateTime Format
public class Boo : ITimeStamps
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Using Unix Format
public class Foo : ITimeStampsUnix
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
}
```

### Using SoftDelete

1. Add `AddTimestamps()` and `AddSoftDeleteFilter()` in your context. see below.

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
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        modelBuilder.AddSoftDeleteFilter(entityTypes);

        base.OnModelCreating(modelBuilder);
    }
}
```

2. Implement a Interface (`ISoftDelete` or `ISoftDeleteUnix`) to your entity.

```cs
using Idam.Libs.EF.Interfaces;

// Using DateTime Format
public class Boo : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DeletedAt { get; set; }
}

// Using Unix Format
public class Foo : ISoftDeleteUnix
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public long? DeletedAt { get; set; }
}
```

The softdelete has restore function, so you can restore the deleted data, see below.
```cs
using Idam.Libs.EF.Extensions;

/// Your context
public class MyDbContext : DbContext
{
    public DbSet<Foo> Foos => Set<Foo>();
}

/// Foo Controller
public class FooController
{
    readonly MyDbContext context;

    public async Task<IActionResult> RestoreAsync(Foo foo)
    {
        Foo restoredFoo = context.Foos.Restore(foo);
        await context.SaveChangesAsync();
        
        return Ok(restoredFoo);
    }
}
```

Also you can ingore the global softdelete filter by using `IgnoreQueryFilters()`. See below.
```cs
public class FooController
{
    public async Task<IActionResult> GetAllDeletedAsync()
    {
        var deletedFoos = await context.Foos
            .IgnoreQueryFilters()
            .Where(x => x.DeletedAt != null)
            .ToListAsync();

        return Ok(deletedFoos);
    }
}
```

### Using IGuidEntity
An Interface to implement Id as Guid instead of int.
```cs
using Idam.Libs.EF.Interfaces;

public class Foo : IGuidEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
```