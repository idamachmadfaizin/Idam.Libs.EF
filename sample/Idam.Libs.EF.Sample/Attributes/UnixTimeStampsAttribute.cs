using Idam.Libs.EF.Attributes;

namespace Idam.Libs.EF.Sample.Attributes;

public class UnixTimeStampsAttribute : TimeStampsAttribute
{
    public override TimeStampsType TimeStampsType { get; set; } = TimeStampsType.Unix;
}
