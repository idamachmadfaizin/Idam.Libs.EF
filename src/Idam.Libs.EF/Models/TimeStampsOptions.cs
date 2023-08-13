namespace Idam.Libs.EF.Models;
public class TimeStampsOptions
{
    public bool UseUtcDateTime { get; set; } = false;
    public string CreatedAtField { get; set; } = "CreatedAt";
    public string UpdatedAtField { get; set; } = "UpdatedAt";
    public string DeletedAtField { get; set; } = "DeletedAt";
}
