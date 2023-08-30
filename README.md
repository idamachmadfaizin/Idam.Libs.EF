# Idam.Libs.EF

[![NuGet](https://img.shields.io/nuget/v/Idam.Libs.EF.svg)](https://www.nuget.org/packages/Idam.Libs.EF) [![.NET](https://github.com/ronnygunawan/RG.RazorMail/actions/workflows/CI.yml/badge.svg)](https://github.com/idamachmadfaizin/Idam.Libs.EF/actions/workflows/test.yml)

[Idam.Libs.EF](https://github.com/idamachmadfaizin/Idam.Libs.EF) is .Net Core (C#) for Entity Framework (EF) Utils.

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

## Features

- Soft delete (DeletedAt).
- Timestamps (CreatedAt, UpdatedAt).
- Custom Timestamps fields.

>Both features support DateTime, UTC DateTime, and [Unix Time Milliseconds](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimemilliseconds?view=net-7.0) format.
>
>Example of Unix Time Milliseconds: [currentmillis.com](https://currentmillis.com)

## Get started

run this command to install

```sh
Install-Package Idam.Libs.EF
```

or

```sh
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
            ChangeTracker.AddTimestamps();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeTracker.AddTimestamps();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
    ```

2. Add an attribute (`TimeStamps` or `TimeStampsUtc` or `TimeStampsUnix`) to your entity. You can also implement an Interface (`ITimeStamps` or `ITimeStampsUnix`) accordion attribute you use.

    ```cs
    using Idam.Libs.EF.Attributes;
    using Idam.Libs.EF.Interfaces;

    /// BaseEntity
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    /// Using DateTime Format
    [TimeStamps]
    public class Doo : BaseEntity, ITimeStamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// Using UTC DateTime Format
    [TimeStampsUtc]
    public class UtcDoo : BaseEntity, ITimeStamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// Using Unix Format
    [TimeStampsUnix]
    public class Foo : ITimeStampsUnix
    {
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
    ```

### Using SoftDelete

1. Add `AddTimestamps()` and `AddSoftDeleteFilter()` in your context.

    ```cs
    using Idam.Libs.EF.Extensions;

    public class MyDbContext : DbContext
    {
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
    ```

2. Add an attribute (`TimeStamps` or `TimeStampsUtc` or `TimeStampsUnix`) to your entity. You can also implement an Interface (`ISoftDelete` or `ISoftDeleteUnix`) accordion attribute you use.

    ```cs
    using Idam.Libs.EF.Attributes;
    using Idam.Libs.EF.Interfaces;

    /// BaseEntity
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    /// Using DateTime Format
    [TimeStamps]
    public class Doo : BaseEntity, ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }

    /// Using UTC DateTime Format
    [TimeStampsUtc]
    public class UtcDoo : BaseEntity, ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }

    /// Using Unix Format
    [TimeStampsUnix]
    public class Foo : BaseEntity, ISoftDeleteUnix
    {
        public long? DeletedAt { get; set; }
    }
    ```

The SoftDelete has a `Restore()` function, so you can restore the deleted data.

```cs
using Idam.Libs.EF.Extensions;

/// Your context
public class MyDbContext : DbContext
{
    public DbSet<Foo> Foos { get; set; }
}

/// Foo Controller
public class FooController
{
    readonly MyDbContext _context;

    public async Task<IActionResult> RestoreAsync(Foo foo)
    {
        Foo restoredFoo = _context.Foos.Restore(foo);
        await context.SaveChangesAsync();
        
        return Ok(restoredFoo);
    }
}
```

Also, you can ignore the global softdelete filter by using `IgnoreQueryFilters()`.

```cs
/// Foo Controller
public class FooController
{
    readonly MyDbContext _context;

    public async Task<IActionResult> GetAllDeletedAsync()
    {
        var deletedFoos = await _context.Foos
            .IgnoreQueryFilters()
            .Where(x => x.DeletedAt != null)
            .ToListAsync();

        return Ok(deletedFoos);
    }
}
```

The SoftDelete has a `ForceRemove()` function, so you can permanently remove the data.

```cs
/// Foo Controller
public class FooController
{
    readonly MyDbContext _context;

    public async Task<IActionResult> ForceRemoveAsync(Foo foo)
    {
        _context.Foos.ForceRemove(foo);
        await context.SaveChangesAsync();
        
        return Ok(restoredFoo);
    }
}
```

The SoftDelete has a `Trashed()` function to check if current data is deleted.

```cs
/// Foo Controller
public class FooController
{
    readonly MyDbContext _context;

    public async Task<IActionResult> ForceRemoveAsync(Foo foo)
    {
        bool isDeletedFoo = foo.Trashed();
        _context.Foos.ForceRemove(foo);
        await context.SaveChangesAsync();
        
        return Ok(restoredFoo);
    }
}
```

> The `Trashed()` function only shows when your entity implements an interface `ISoftDelete` or `ISoftDeleteUnix`.

### Using Custom TimeStamps fields.

By default, the TimeStamps attribute uses CreatedAt, UpdatedAt, and DeletedAt as field names. It's possible to customize the TimeStamps fields.

1. Create your own TimeStamps attribute.

    ```cs
    public class MyTimeStampsAttribute : TimeStampsAttribute
    {
        public override TimeStampsType TimeStampsType { get; set; } = TimeStampsType.UtcDateTime;
        public override string? CreatedAtField { get; set; } = "AddedAt";
        public override string? UpdatedAtField { get; set; } = "EditedAt";
        public override string? DeletedAtField { get; set; } = "RemovedAt";
    }
    ```

2. Add the new attribute to your entity.

    ```cs
    [MyTimeStamps]
    public class Doo : BaseEntity
    {
        public DateTime AddedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
    }
    ```

    > Tips: Create your interface according to your own TimeStamps attribute.

### Using Few TimeStamps attribute

You can use just a few TimeStamps fields by filling in null or string empty.

1. Create your own TimeStamps attribute.

    ```cs
    public class MyTimeStampsAttribute : TimeStampsAttribute
    {
        public override TimeStampsType TimeStampsType { get; set; } = TimeStampsType.UtcDateTime;
        public override string? CreatedAtField { get; set; } = "";
        public override string? UpdatedAtField { get; set; } = "EditedAt";
        public override string? DeletedAtField { get; set; } = "RemovedAt";
    }
    ```

2. Add the new attribute to your entity.

    ```cs
    [MyTimeStamps]
    public class Doo : BaseEntity
    {
        public DateTime EditedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
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

## Migrating
Migrating from 2.1.0

1. Add [TimeStamps] or [TimeStampsUnix] or [TimeStampsUtc] attribute to your entities.