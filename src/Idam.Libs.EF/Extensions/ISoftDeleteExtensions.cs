using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;
public static class ISoftDeleteExtensions
{
    /// <summary>
    /// Determines whether this instance is deleted.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    ///   <c>true</c> if the specified entity is deleted; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static bool Trashed(this ISoftDeleteBase entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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

        var value = deletedAtProperty!.GetValue(entity);

        return value is not null;
    }
}
