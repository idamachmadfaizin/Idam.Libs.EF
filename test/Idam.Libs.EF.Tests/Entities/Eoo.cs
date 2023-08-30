using Idam.Libs.EF.Attributes;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for using only UpdatedAt timestamps fields.
/// </summary>
[TimeStamps(CreatedAtField = null, DeletedAtField = null)]
public class Eoo : BaseEntity
{
    public DateTime UpdatedAt { get; set; }
}
