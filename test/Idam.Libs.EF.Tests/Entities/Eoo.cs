using Idam.Libs.EF.Attributes;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for using only few timestamps fields.
/// </summary>
[TimeStamps(CreatedAtField = null, DeletedAtField = null)]
public class Eoo : BaseEntity
{
    public DateTime UpdatedAt { get; set; }
}
