using Idam.Libs.EF.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;
public static class DbSetExtensions
{
    /// <summary>
    /// Restore deleted data.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="dbSet"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TEntity Restore<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(dbSet, nameof(dbSet));

        Type entityType = entity.GetType();
        TimeStampsAttribute? timeStampsAttribute = entityType.GetCustomAttribute<TimeStampsAttribute>();

        ArgumentNullException.ThrowIfNull(timeStampsAttribute, nameof(timeStampsAttribute));

        PropertyInfo? deletedAtProperty = entityType.GetProperty(timeStampsAttribute.DeletedAtField);

        InvalidCastValidationException.ThrowIfInvalid(timeStampsAttribute.DeletedAtField, entityType, timeStampsAttribute);

        deletedAtProperty!.SetValue(entity, null, null);

        return entity;
    }
        {
            TimeStampsType.Unix => typeof(long?),
            _ => typeof(DateTime?),
        };
        PropertyInfo? deletedAtProperty = type.GetProperty(timeStampsAttribute.DeletedAtField);

        if (deletedAtProperty is null || deletedAtProperty.PropertyType != timeStampsType)
        {
            throw new InvalidCastException($"The property '{timeStampsAttribute.DeletedAtField}' in {type.Name} is not of type {timeStampsType.Name}.");
        }

        deletedAtProperty.SetValue(entity, null, null);

        return entity;
    }
}
