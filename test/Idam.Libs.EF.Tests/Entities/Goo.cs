using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Entity for normal case using Utc data type.
/// </summary>
/// <seealso cref="BaseEntity" />
/// <seealso cref="ITimeStamps" />
/// <seealso cref="ISoftDelete" />
[TimeStampsUtc]
public class Goo : BaseEntity, ITimeStamps, ISoftDelete
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
