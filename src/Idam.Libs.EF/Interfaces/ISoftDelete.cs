namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// SoftDelete interface using DateTime format
/// </summary>
public interface ISoftDelete : ISoftDeleteBase
{
    DateTime? DeletedAt { get; set; }
}