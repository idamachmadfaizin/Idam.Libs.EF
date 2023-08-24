using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Entity for normal case using Unix data type.
/// </summary>
/// <seealso cref="IGuidEntity" />
/// <seealso cref="ITimeStampsUnix" />
/// <seealso cref="ISoftDeleteUnix" />
[TimeStampsUnix]
public class Foo : BaseEntity, ITimeStampsUnix, ISoftDeleteUnix
{
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
    public long? DeletedAt { get; set; }
}