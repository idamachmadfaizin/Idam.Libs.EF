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

        // current datetime in unix format
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (entityEntry.Entity is ITimeStamps timeStamps)
        {
            switch (entityEntry.State)
            {
                case EntityState.Modified:
                    timeStamps.UpdatedAt = now;
                    break;

                case EntityState.Added:
                    timeStamps.CreatedAt = now;
                    timeStamps.UpdatedAt = now;
                    break;

                default:
                    break;
            }
        }

        if (entityEntry.Entity is ISoftDelete softDelete && entityEntry.State == EntityState.Deleted)
        {
            entityEntry.State = EntityState.Modified;
            softDelete.DeletedAt = now;
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt not null
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="mutable"></param>
    public static void AddSoftDeleteFilter(this ModelBuilder builder, IMutableEntityType? mutable)
    {
        if (mutable is not null && typeof(ISoftDelete).IsAssignableFrom(mutable.ClrType))
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
        where TEntity : class, ISoftDelete
    {
        if (dbSet is null) throw new ArgumentNullException(nameof(dbSet));

        if (entity is not null and ISoftDelete)
            entity.DeletedAt = null;
        return entity!;
    }
}