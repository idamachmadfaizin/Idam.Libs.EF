namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// SoftDelete interface using DateTime format
/// </summary>
public interface ISoftDelete
{
    DateTime? DeletedAt { get; set; }
}

/// <summary>
/// SoftDelete interface using unix format
/// </summary>
public interface ISoftDeleteUnix
{
    long? DeletedAt { get; set; }
}
