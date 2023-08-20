using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Foo entity
/// </summary>
[TimeStamps(TimeStampsType = TimeStampsType.Unix)]
public class Foo : IGuidEntity
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