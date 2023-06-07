using Idam.Libs.EF.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace Idam.Libs.EF.Extensions;

/// <summary>
/// DbContext extension
/// </summary>
public static class DbContextExtension
{
    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="entityEntries"></param>
    public static void AddTimestamps(this IEnumerable<EntityEntry> entityEntries)
    {
        foreach (var entityEntry in entityEntries)
        {
            AddTimestamps(entityEntry);
        }
    }

    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="entityEntry"></param>
    public static void AddTimestamps(this EntityEntry? entityEntry)
    {
        if (entityEntry == null) return;

        // current datetime
        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var now = DateTime.Now;

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
                entityEntry.State = EntityState.Modified;

                if (entityEntry.Entity is ISoftDelete softDelete)
                {
                    softDelete.DeletedAt = now;
                }
                else if (entityEntry.Entity is ISoftDeleteUnix softDeleteUnix)
                {
                    softDeleteUnix.DeletedAt = nowUnix;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt not null
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="mutable"></param>
    public static void AddSoftDeleteFilter(this ModelBuilder builder, IMutableEntityType? mutable)
    {
        if (mutable is null)
        {
            return;
        }

        if (typeof(ISoftDelete).IsAssignableFrom(mutable.ClrType) || typeof(ISoftDeleteUnix).IsAssignableFrom(mutable.ClrType))
        {
            var parameter = Expression.Parameter(mutable.ClrType, "e");
            var body = Expression.Equal(
                Expression.Call(typeof(Microsoft.EntityFrameworkCore.EF), nameof(Microsoft.EntityFrameworkCore.EF.Property), new[] { typeof(long?) }, parameter, Expression.Constant(nameof(ISoftDelete.DeletedAt))),
                Expression.Constant(null));
            var expression = Expression.Lambda(body, parameter);

            builder.Entity(mutable.ClrType).HasQueryFilter(expression);
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt not null
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="mutables"></param>
    public static void AddSoftDeleteFilter(this ModelBuilder builder, IEnumerable<IMutableEntityType>? mutables)
    {
        if (mutables is not null)
        {
            foreach (var mutable in mutables)
            {
                builder.AddSoftDeleteFilter(mutable);
            }
        }
    }

    /// <summary>
    /// DbSet extension to restore deleted data
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="dbSet"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TEntity Restore<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class, ISoftDeleteBase
    {
        if (dbSet is null)
        {
            throw new ArgumentNullException(nameof(dbSet));
        }
        if (entity is not ISoftDeleteBase)
        {
            throw new ArgumentException($"{nameof(entity)} must be ISoftDelete or ISoftDeleteUnix");
        }

        if (entity is ISoftDelete softDelete)
        {
            softDelete.DeletedAt = null;
        }
        else if (entity is ISoftDeleteUnix softDeleteUnix)
        {
            softDeleteUnix.DeletedAt = null;
        }

        return entity;
    }
}