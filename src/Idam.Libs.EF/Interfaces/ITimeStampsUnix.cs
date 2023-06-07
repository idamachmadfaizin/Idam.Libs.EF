namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// Timestamps interface using unix format
/// </summary>
public interface ITimeStampsUnix
{
    long CreatedAt { get; set; }
    long UpdatedAt { get; set; }
}
