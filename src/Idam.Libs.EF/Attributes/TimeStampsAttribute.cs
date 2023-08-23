namespace Idam.Libs.EF.Attributes;

/// <summary>
/// TimeStamps type
/// </summary>
public enum TimeStampsType
{
    DateTime,
    Unix,
    UtcDateTime,
}

/// <summary>
/// TimeStamps Attribute.
/// You can inherit this class to avoid declaring fields every time you use this attribute.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class TimeStampsAttribute : Attribute
{
    public virtual TimeStampsType TimeStampsType { get; set; } = TimeStampsType.DateTime;
    public virtual string? CreatedAtField { get; set; } = "CreatedAt";
    public virtual string? UpdatedAtField { get; set; } = "UpdatedAt";
    public virtual string? DeletedAtField { get; set; } = "DeletedAt";
}

/// <summary>
/// TimeStamps Unix Attribute.
/// </summary>
/// <seealso cref="TimeStampsAttribute" />
public sealed class TimeStampsUnixAttribute : TimeStampsAttribute
{
    public override TimeStampsType TimeStampsType { get; set; } = TimeStampsType.Unix;
}

/// <summary>
/// TimeStamps Utc DateTime Attribute.
/// </summary>
/// <seealso cref="TimeStampsAttribute" />
public sealed class TimeStampsUtcAttribute : TimeStampsAttribute
{
    public override TimeStampsType TimeStampsType { get; set; } = TimeStampsType.UtcDateTime;
}

/// <summary>
/// TimeStamps Type Extensions
/// </summary>
internal static class TimeStampsTypeExtensions
{
    /// <summary>
    /// Corresponding data type for the given TimeStampsType enum value.
    /// </summary>
    /// <param name="timeStampsType">Type of the time stamps.</param>
    /// <returns></returns>
    /// <seealso cref="TimeStampsType" />
    public static Type GetMapType(this TimeStampsType timeStampsType)
    {
        return timeStampsType switch
        {
            TimeStampsType.Unix => typeof(long),
            _ => typeof(DateTime),
        };
    }

    /// <summary>
    /// Corresponding data type for the given TimeStampsType enum value.
    /// </summary>
    /// <param name="timeStampsType">Type of the time stamps.</param>
    /// <returns></returns>
    /// <seealso cref="TimeStampsType" />
    public static Type GetNullableMapType(this TimeStampsType timeStampsType)
    {
        return timeStampsType switch
        {
            TimeStampsType.Unix => typeof(long?),
            _ => typeof(DateTime?),
        };
    }

    /// <summary>
    /// Corresponding date value as DateTime or long type for given TimeStampsType enum value.
    /// </summary>
    /// <param name="timeStampsType">Type of the time stamps.</param>
    /// <returns>UTC DateTime or DateTime or long.</returns>
    /// <seealso cref="TimeStampsType" />
    public static object GetMapValue(this TimeStampsType timeStampsType)
    {
        return timeStampsType switch
        {
            TimeStampsType.Unix => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            TimeStampsType.UtcDateTime => DateTime.UtcNow,
            _ => DateTime.Now,
        };
    }
}