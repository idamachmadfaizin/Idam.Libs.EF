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
        object now = timeStampsAttribute.TimeStampsType.GetMapValue();

        var entityType = entityEntry.Entity.GetType();
        var createdAtProperty = entityType.GetProperty(timeStampsAttribute.CreatedAtField);
        var updatedAtProperty = entityType.GetProperty(timeStampsAttribute.UpdatedAtField);
        var deletedAtProperty = entityType.GetProperty(timeStampsAttribute.DeletedAtField);

        switch (entityEntry.State)
        {
            case EntityState.Modified:
                ThrowIfInvalid(timeStampsAttribute.UpdatedAtField, entityType, timeStampsAttribute);

                updatedAtProperty!.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Added:
                ThrowIfInvalid(timeStampsAttribute.CreatedAtField, entityType, timeStampsAttribute);
                ThrowIfInvalid(timeStampsAttribute.UpdatedAtField, entityType, timeStampsAttribute);

                createdAtProperty!.SetValue(entityEntry.Entity, now, null);
                updatedAtProperty!.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Deleted:
                ThrowIfInvalid(timeStampsAttribute.DeletedAtField, entityType, timeStampsAttribute);

                var value = deletedAtProperty!.GetValue(entityEntry.Entity);

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

    /// <summary>
    /// Throw when entity property not valid
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="entityType"></param>
    /// <param name="timeStampsAttribute"></param>
    /// <exception cref="InvalidCastException"></exception>
    private static void ThrowIfInvalid(string propertyName, Type entityType, TimeStampsAttribute timeStampsAttribute)
    {
        Type timeStampsType = timeStampsAttribute.TimeStampsType.GetMapType();

        if (propertyName.Equals(timeStampsAttribute.DeletedAtField))
        {
            timeStampsType = typeof(Nullable<>).MakeGenericType(timeStampsType);
        }

        var property = entityType.GetProperty(propertyName);

        if (property is null || property.PropertyType != timeStampsType)
        {
            throw new InvalidCastException($"The property '{propertyName}' in {entityType.Name} is not of type {timeStampsType.Name}.");
        }
    }
}