namespace Infrastructure.DateTimeHelpers.DateTimeProviders;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IranianTimeZone);
    public TimeZoneInfo IranianTimeZone => TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");


    public override string ToString()
        => $"Current timeZone : {TimeZoneInfo.Local.DisplayName} - DateTime now : {Now}";
}
