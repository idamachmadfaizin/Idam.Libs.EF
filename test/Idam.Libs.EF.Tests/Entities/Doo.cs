using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Entity for error data type invalid test.
/// </summary>
/// <seealso cref="IGuidEntity" />
/// <seealso cref="ITimeStamps" />
[TimeStamps]
public class Doo : BaseEntity, ITimeStamps
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
