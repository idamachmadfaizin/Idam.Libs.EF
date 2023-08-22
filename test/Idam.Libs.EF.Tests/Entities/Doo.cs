using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for error data type invalid test.
/// </summary>
/// <seealso cref="IGuidEntity" />
[TimeStamps]
public class Doo : IGuidEntity, ITimeStamps
{
    public Guid Id { get; set; }
    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DeletedAt { get; set; }
}
