namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// Timestamps interface using unix format
/// </summary>
public interface ITimeStamps
{
    long CreatedAt { get; set; }
    long UpdatedAt { get; set; }
}

/// <summary>
/// SoftDelete interface using unix format
/// </summary>
public interface ISoftDelete
{
    long? DeletedAt { get; set; }
}