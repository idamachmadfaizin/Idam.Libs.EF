namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// An Interface reprecentation of Id as Guid instead of int
/// </summary>
public interface IGuidEntity
{
    Guid Id { get; set; }
}