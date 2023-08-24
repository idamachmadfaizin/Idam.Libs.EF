using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;

namespace Idam.Libs.EF.Tests.Entities;

/// <summary>
/// Entity for error field doesn't exist test.
/// </summary>
/// <seealso cref="IGuidEntity" />
[TimeStamps]
public class Aoo : BaseEntity
{
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
