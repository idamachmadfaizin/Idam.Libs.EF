using Idam.Libs.EF.Interfaces;
using Idam.Libs.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Idam.Libs.EF.Extensions;

/// <summary>
/// DbContext extension
/// </summary>
public static class DbContextExtensions
{
    private static TimeStampsOptions _timeStampsOptions = new();

    public static TimeStampsOptions TimeStampsOptions { get => _timeStampsOptions; }

    /// <summary>
    /// Inject IServiceProvider to this class
    /// </summary>
    /// <param name="provider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Configure(IServiceProvider provider)
    {
        if (provider is null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        _timeStampsOptions = GetTimeStampsOptions(provider);
    }

    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="changeTracker"></param>
    public static void AddTimestamps(this ChangeTracker changeTracker)
    {
        foreach (var entityEntry in changeTracker.Entries())
        {
            AddTimestamps(entityEntry);
        }
    }

    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="entityEntry"></param>
    private static void AddTimestamps(this EntityEntry? entityEntry)
    {
        if (entityEntry is null) return;

        // current datetime
        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var now = _timeStampsOptions.UseUtcDateTime ? DateTime.UtcNow : DateTime.Now;

        switch (entityEntry.State)
        {
            case EntityState.Modified:
                if (entityEntry.Entity is ITimeStamps timeStampsModified)
                {
                    timeStampsModified.UpdatedAt = now;
                }
                else if (entityEntry.Entity is ITimeStampsUnix timeStampsUnix)
                {
                    timeStampsUnix.UpdatedAt = nowUnix;
                }
                break;

            case EntityState.Added:
                if (entityEntry.Entity is ITimeStamps timeStampsAdded)
                {
                    timeStampsAdded.CreatedAt = now;
                    timeStampsAdded.UpdatedAt = now;
                }
                else if (entityEntry.Entity is ITimeStampsUnix timeStampsUnix)
                {
                    timeStampsUnix.CreatedAt = nowUnix;
                    timeStampsUnix.UpdatedAt = nowUnix;
                }
                break;

            case EntityState.Deleted:
                if (entityEntry.Entity is ISoftDelete softDelete && softDelete.DeletedAt is null)
                {
                    entityEntry.State = EntityState.Modified;
                    softDelete.DeletedAt = now;
                }
                else if (entityEntry.Entity is ISoftDeleteUnix softDeleteUnix && softDeleteUnix.DeletedAt is null)
                {
                    entityEntry.State = EntityState.Modified;
                    softDeleteUnix.DeletedAt = nowUnix;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Get TimeStampsOptions from DI
    /// </summary>
    /// <param name="_serviceProvider"></param>
    /// <returns></returns>
    private static TimeStampsOptions GetTimeStampsOptions(IServiceProvider? provider)
    {
        if (provider is not null)
        {
            var optionsTimeStampsOptions = provider.GetRequiredService<IOptions<TimeStampsOptions>>();
            if (optionsTimeStampsOptions is not null)
            {
                return optionsTimeStampsOptions.Value;
            }
        }

        return new TimeStampsOptions();
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt not null
    /// </summary>
    /// <param name="builder"></param>
    public static void AddSoftDeleteFilter(this ModelBuilder builder)
    {
        var mutables = builder.Model.GetEntityTypes();

        if (mutables is not null)
        {
            foreach (var mutable in mutables)
            {
                builder.AddSoftDeleteFilter(mutable);
            }
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt not null
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="mutable"></param>
    private static void AddSoftDeleteFilter(this ModelBuilder builder, IMutableEntityType? mutable)
    {
        if (mutable is null)
        {
            return;
        }

        if (typeof(ISoftDelete).IsAssignableFrom(mutable.ClrType) || typeof(ISoftDeleteUnix).IsAssignableFrom(mutable.ClrType))
        {
            var parameter = Expression.Parameter(mutable.ClrType, "e");
            Type[] typeArguments = new[] { typeof(ISoftDelete).IsAssignableFrom(mutable.ClrType) ? typeof(DateTime?) : typeof(long?) };

            var body = Expression.Equal(
                Expression.Call(typeof(Microsoft.EntityFrameworkCore.EF), nameof(Microsoft.EntityFrameworkCore.EF.Property), typeArguments, parameter, Expression.Constant(nameof(ISoftDelete.DeletedAt))),
                Expression.Constant(null));

            var expression = Expression.Lambda(body, parameter);

            builder.Entity(mutable.ClrType).HasQueryFilter(expression);
        }
    }
}