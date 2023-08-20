using Idam.Libs.EF.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;

/// <summary>
/// DbContext extension
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="changeTracker"></param>
    public static void AddTimestamps(this ChangeTracker changeTracker)
    {
        foreach (var entityEntry in changeTracker.Entries())
        {
            var timeStampsAttribute = entityEntry.Entity.GetType().GetCustomAttribute<TimeStampsAttribute>();

            if (timeStampsAttribute is not null)
            {
                AddTimestamps(entityEntry, timeStampsAttribute);
            }
        }
    }

    /// <summary>
    /// Add Unix timestamps to the TimeStamps Entity when state is Added or Modified or Deleted
    /// </summary>
    /// <param name="entityEntry"></param>
    private static void AddTimestamps(this EntityEntry? entityEntry, TimeStampsAttribute timeStampsAttribute)
    {
        if (entityEntry is null) return;

        // current datetime
        object now = timeStampsAttribute.TimeStampsType switch
        {
            TimeStampsType.Unix => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            TimeStampsType.UtcDateTime => DateTime.UtcNow,
            _ => DateTime.Now,
        };

        Type timeStampsType = timeStampsAttribute.TimeStampsType switch
        {
            TimeStampsType.Unix => typeof(long),
            _ => typeof(DateTime),
        };

        var type = entityEntry.Entity.GetType();
        var createdAtProperty = type.GetProperty(timeStampsAttribute.CreatedAtField);
        var updatedAtProperty = type.GetProperty(timeStampsAttribute.UpdatedAtField);
        var deletedAtProperty = type.GetProperty(timeStampsAttribute.DeletedAtField);

        switch (entityEntry.State)
        {
            case EntityState.Modified:
                if (updatedAtProperty is null || updatedAtProperty.PropertyType != timeStampsType)
                {
                    throw new InvalidCastException($"The property '{timeStampsAttribute.UpdatedAtField}' in {type.Name} is not of type {timeStampsType.Name}.");
                }

                updatedAtProperty.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Added:
                if (createdAtProperty is null || createdAtProperty.PropertyType != timeStampsType)
                {
                    throw new InvalidCastException($"The property '{timeStampsAttribute.CreatedAtField}' in {type.Name} is not of type {timeStampsType.Name}.");
                }
                if (updatedAtProperty is null || updatedAtProperty.PropertyType != timeStampsType)
                {
                    throw new InvalidCastException($"The property '{timeStampsAttribute.UpdatedAtField}' in {type.Name} is not of type {timeStampsType.Name}.");
                }

                createdAtProperty.SetValue(entityEntry.Entity, now, null);
                updatedAtProperty.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Deleted:
                if (deletedAtProperty is null)
                {
                    return;
                }

                timeStampsType = typeof(Nullable<>).MakeGenericType(timeStampsType);
                if (deletedAtProperty is null || deletedAtProperty.PropertyType != timeStampsType)
                {
                    throw new InvalidCastException($"The property '{timeStampsAttribute.DeletedAtField}' in {type.Name} is not of type {timeStampsType.Name}.");
                }

                var value = deletedAtProperty.GetValue(entityEntry.Entity);

                if (value is null)
                {
                    entityEntry.State = EntityState.Modified;
                    deletedAtProperty.SetValue(entityEntry.Entity, now, null);
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

        var timeStampsAttribute = mutable.ClrType.GetCustomAttribute<TimeStampsAttribute>();

        if (timeStampsAttribute is null)
        {
            return;
        }

        var parameter = Expression.Parameter(mutable.ClrType, "e");
        Type[] typeArguments = timeStampsAttribute.TimeStampsType switch
        {
            TimeStampsType.Unix => new[] { typeof(long?) },
            _ => new[] { typeof(DateTime?) },
        };

        var body = Expression.Equal(
                Expression.Call(typeof(Microsoft.EntityFrameworkCore.EF), nameof(Microsoft.EntityFrameworkCore.EF.Property), typeArguments, parameter, Expression.Constant(timeStampsAttribute.DeletedAtField)),
                Expression.Constant(null));

        var expression = Expression.Lambda(body, parameter);

        builder.Entity(mutable.ClrType).HasQueryFilter(expression);
    }
}