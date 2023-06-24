namespace Idam.Libs.EF.Tests.Ekstensions;
internal static class DateTimeExtension
{
    public static long ToUnixTimeMilliseconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
}
