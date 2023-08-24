using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for custom timestamps fields.
/// </summary>
/// <seealso cref="IGuidEntity" />
[TimeStamps(CreatedAtField = nameof(AddedAt), UpdatedAtField = nameof(EditedAt), DeletedAtField = nameof(RemovedAt))]
public class Coo : BaseEntity
{
    public DateTime AddedAt { get; set; }
    public DateTime EditedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}
