namespace Idam.Libs.EF.Attributes;

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