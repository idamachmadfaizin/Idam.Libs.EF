using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Foo entity
/// </summary>
public class Foo : IGuidEntity, ITimeStampsUnix, ISoftDeleteUnix
{
    public Guid Id { get; set; }

    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public long? DeletedAt { get; set; }
}