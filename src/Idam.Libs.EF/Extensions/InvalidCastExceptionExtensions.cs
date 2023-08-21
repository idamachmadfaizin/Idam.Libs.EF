using Idam.Libs.EF.Attributes;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;
internal static class InvalidCastValidationException
{
    /// <summary>
    /// Throw when entity property not valid.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="entityType"></param>
    /// <param name="timeStampsAttribute"></param>
    /// <exception cref="InvalidCastValidationException"></exception>
    public static void ThrowIfInvalidTimeStamps(string propertyName, Type entityType, TimeStampsAttribute timeStampsAttribute)
    {
        Type timeStampsType = timeStampsAttribute.TimeStampsType.GetMapType();

        if (propertyName.Equals(timeStampsAttribute.DeletedAtField))
        {
            timeStampsType = typeof(Nullable<>).MakeGenericType(timeStampsType);
        }

        PropertyInfo? property = entityType.GetProperty(propertyName);

        if (property is null || property.PropertyType != timeStampsType)
        {
            throw new InvalidCastException($"The property '{propertyName}' in {entityType.Name} is not of type {timeStampsType.Name}.");
        }
    }
}
