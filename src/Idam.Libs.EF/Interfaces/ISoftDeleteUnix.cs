namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// SoftDelete interface using unix format
/// </summary>
public interface ISoftDeleteUnix
{
    long? DeletedAt { get; set; }
}
