using Idam.Libs.EF.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;
public static class DbSetExtensions
{
    /// <summary>
    /// Restores the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbSet">The database set.</param>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static TEntity Restore<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(dbSet, nameof(dbSet));

        Type entityType = entity.GetType();
        TimeStampsAttribute? timeStampsAttribute = entityType.GetCustomAttribute<TimeStampsAttribute>();

        ArgumentNullException.ThrowIfNull(timeStampsAttribute, nameof(timeStampsAttribute));

        var useDeletedAtField = !string.IsNullOrWhiteSpace(timeStampsAttribute.DeletedAtField);
        if (useDeletedAtField == false)
        {
            throw new Exception($"The entity '{entityType.Name}' not implement SoftDelete.");
        }

        InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.DeletedAtField, entityType, timeStampsAttribute);

        PropertyInfo? deletedAtProperty = useDeletedAtField ? entityType.GetProperty(timeStampsAttribute.DeletedAtField!) : null;

        deletedAtProperty!.SetValue(entity, null, null);

        return entity;
    }

    /// <summary>
    /// Forces the remove.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbSet">The database set.</param>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static EntityEntry<TEntity> ForceRemove<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(dbSet, nameof(dbSet));

        Type entityType = entity.GetType();
        TimeStampsAttribute? timeStampsAttribute = entityType.GetCustomAttribute<TimeStampsAttribute>();

        ArgumentNullException.ThrowIfNull(timeStampsAttribute, nameof(timeStampsAttribute));

        var useDeletedAtField = !string.IsNullOrWhiteSpace(timeStampsAttribute.DeletedAtField);
        if (useDeletedAtField == false)
        {
            throw new Exception($"The entity '{entityType.Name}' not implement SoftDelete.");
        }

        InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.DeletedAtField, entityType, timeStampsAttribute);

        PropertyInfo? deletedAtProperty = useDeletedAtField ? entityType.GetProperty(timeStampsAttribute.DeletedAtField!) : null;

        var now = timeStampsAttribute.TimeStampsType.GetMapValue();

        deletedAtProperty!.SetValue(entity, now, null);

        return dbSet.Remove(entity);
    }
}
