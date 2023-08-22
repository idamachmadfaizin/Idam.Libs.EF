using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Entity for error field doesn't exist test.
/// </summary>
/// <seealso cref="IGuidEntity" />
[TimeStamps]
public class Aoo : IGuidEntity
{
    public Guid Id { get; set; }
    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }

    public DateTime AddedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
