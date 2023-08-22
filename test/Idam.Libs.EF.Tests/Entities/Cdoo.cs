using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for custom timestamps fields.
/// </summary>
/// <seealso cref="IGuidEntity" />
[TimeStamps(CreatedAtField = nameof(AddedAt), UpdatedAtField = nameof(EditedAt), DeletedAtField = nameof(RemovedAt))]
public class Cdoo : IGuidEntity
{
    public Guid Id { get; set; }
    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }

    public DateTime AddedAt { get; set; }

    public DateTime EditedAt { get; set; }

    public DateTime? RemovedAt { get; set; }
}
