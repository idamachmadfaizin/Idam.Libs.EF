﻿namespace Idam.Libs.EF.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TimeStampsAttribute : Attribute
{
    public virtual TimeStampsType TimeStampsType { get; set; } = TimeStampsType.DateTime;
    public virtual string CreatedAtField { get; set; } = "CreatedAt";
    public virtual string UpdatedAtField { get; set; } = "UpdatedAt";
    public virtual string DeletedAtField { get; set; } = "DeletedAt";
}

/// <summary>
/// TimeStamps type
/// </summary>
public enum TimeStampsType
{
    Unix,
    UtcDateTime,
    DateTime
}

/// <summary>
/// TimeStamps Type Extensions
/// </summary>
static class TimeStampsTypeExtensions
{
    /// <summary>
    /// Corresponding data type for the given TimeStampsType enum value.
    /// </summary>
    /// <param name="timeStampsType"></param>
    /// <returns>long or DateTime data type.</returns>
    public static Type GetMapType(this TimeStampsType timeStampsType)
    {
        return timeStampsType switch
        {
            TimeStampsType.Unix => typeof(long),
            _ => typeof(DateTime),
        };
    }

    /// <summary>
    /// Corresponding date value as DateTime or long type for given TimeStampsType enum value.
    /// </summary>
    /// <param name="timeStampsType"></param>
    /// <returns>UTC DateTime or DateTime or long.</returns>
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