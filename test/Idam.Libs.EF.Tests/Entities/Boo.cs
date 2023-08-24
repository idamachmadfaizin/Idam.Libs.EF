using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Entity for normal case using DateTime data type.
/// </summary>
/// <seealso cref="IGuidEntity" />
/// <seealso cref="ITimeStamps" />
/// <seealso cref="ISoftDelete" />
[TimeStamps]
public class Boo : BaseEntity, ITimeStamps, ISoftDelete
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
}
