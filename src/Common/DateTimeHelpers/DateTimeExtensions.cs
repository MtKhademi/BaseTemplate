namespace Common.DateTimeHelpers;

public static class DateTimeExtensions
{
    #region Persian DateTime Methods

    public static int GetPersianYear(this DateTime dt) => new PersianCalendar().GetYear(dt);
    public static int GetPersianMonth(this DateTime dt) => new PersianCalendar().GetMonth(dt);
    public static int GetPersianDay(this DateTime dt) => new PersianCalendar().GetDayOfMonth(dt);
    public static int GetPersianHour(this DateTime dt) => new PersianCalendar().GetHour(dt);
    public static int GetPersianMinute(this DateTime dt) => new PersianCalendar().GetMinute(dt);
    public static int GetPersianSecond(this DateTime dt) => new PersianCalendar().GetSecond(dt);

    public static string GetPersianDate(this DateTime dt, string separator = "/") =>
        $"{dt.GetPersianYear():0000}{separator}{dt.GetPersianMonth():00}{separator}{dt.GetPersianDay():00}";

    public static string GetPersianTime(this DateTime dt, string separator = ":") =>
        $"{dt.GetPersianHour():00}{separator}{dt.GetPersianMinute():00}{separator}00";

    public static string GetPersianDateTime(this DateTime dt) => dt.GetPersianDateTime("/", ":");
    public static string GetPersianDateTime(this DateTime? dt) =>
        dt.HasValue ? dt.Value.GetPersianDateTime("/", ":") : string.Empty;

    public static string GetPersianDateTime(this DateTime dt, string dateSeparator = "/", string timeSeparator = ":", string dateTimeSeparator = " ") =>
        $"{dt.GetPersianDate(dateSeparator)}{dateTimeSeparator}{dt.GetPersianTime(timeSeparator)}";

    #endregion

    #region Day Start/End

    public static DateTime StartDay(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
    public static DateTime EndDay(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

    #endregion

    #region String Formatting

    public static string GetStringDateTime(this DateTime dt, string dateSeparator = "/", string dateTimeSeparator = " ", string timeSeparator = ":") =>
        $"{dt.GetStringDate(dateSeparator)}{dateTimeSeparator}{dt.GetStringTime(timeSeparator)}";

    public static string GetStringDate(this DateTime dt, string dateSeparator = "/") =>
        $"{dt.Year:0000}{dateSeparator}{dt.Month:00}{dateSeparator}{dt.Day:00}";

    public static string GetStringTime(this DateTime dt, string timeSeparator = ":") =>
        $"{dt.Hour:00}{timeSeparator}{dt.Minute:00}{timeSeparator}{dt.Second:00}";

    #endregion

    #region Miladi DateTime Conversion

    public static DateTime ConvertToDateTimeFromMiladiDateTime(this string input, string dateTimeSeparator = " ", string dateSeparator = "/", string timeSeparator = ":")
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new NotValidDataException($"Cannot convert '{input}' to DateTime. Expected format: yyyy{dateSeparator}MM{dateSeparator}dd{dateTimeSeparator}HH{timeSeparator}mm{timeSeparator}ss");

        try
        {
            if (input.Contains(dateTimeSeparator))
            {
                var parts = input.Trim().Split(dateTimeSeparator);
                var dateParts = parts[0].Split(dateSeparator);
                var timeParts = parts[1].Split(timeSeparator);
                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                int second = int.Parse(timeParts[2].Split('.')[0]);
                return new DateTime(year, month, day, hour, minute, second);
            }
            else
            {
                var dateParts = input.Split(dateSeparator);
                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);
                return new DateTime(year, month, day, 0, 0, 0);
            }
        }
        catch
        {
            throw new NotValidDataException($"Cannot convert '{input}' to DateTime. Expected format: yyyy{dateSeparator}MM{dateSeparator}dd{dateTimeSeparator}HH{timeSeparator}mm{timeSeparator}ss");
        }
    }

    public static DateTime ConvertToDateFromMiladiDate(this string input, string dateSeparator = "-", string dateTimeSeparator = "T", string timeSeparator = ":") =>
        input.ConvertToDateTimeFromMiladiDateTime(dateTimeSeparator, dateSeparator, timeSeparator).Date;

    #endregion

    #region Persian DateTime Conversion

    public static DateTime ConvertToDateTimeFromPersianDateTime(this string input, string dateTimeSeparator = " ", string dateSeparator = "/", string timeSeparator = ":")
    {
        var persianCalendar = new PersianCalendar();
        var parts = input.Trim().Split(dateTimeSeparator);
        var dateParts = parts[0].Split(dateSeparator);
        var timeParts = parts[1].Split(timeSeparator);
        int year = int.Parse(dateParts[0]);
        int month = int.Parse(dateParts[1]);
        int day = int.Parse(dateParts[2]);
        int hour = int.Parse(timeParts[0]);
        int minute = int.Parse(timeParts[1]);

        if (parts.Length == 3 && parts[2].Trim().ToLower() == "pm" && hour < 12)
            hour += 12;

        return new DateTime(year, month, day, hour, minute, 0, 0, persianCalendar, DateTimeKind.Local);
    }

    public static DateTime ConvertToDateFromPersianDate(this string input, string dateSeparator = "/")
    {
        var persianCalendar = new PersianCalendar();
        var dateParts = input.Trim().ToEnglishNumber().Split(dateSeparator);
        int year = int.Parse(dateParts[0]);
        int month = int.Parse(dateParts[1]);
        int day = int.Parse(dateParts[2]);
        return new DateTime(year, month, day, 0, 0, 0, persianCalendar);
    }

    #endregion

    #region Unix Timestamp Conversion

    public static double ConvertToUnixTimestamp(this DateTime date) =>
        date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

    public static double? ConvertToUnixTimestamp(this DateTime? date, DateTime? defaultValue = null) =>
        date.HasValue ? date.Value.ConvertToUnixTimestamp() :
        defaultValue.HasValue ? defaultValue.Value.ConvertToUnixTimestamp() : null;

    public static DateTime ConvertToDateTimeFromUnixTimestamp(this string timeStamp)
    {
        if (string.IsNullOrWhiteSpace(timeStamp))
            throw new Exception("Timestamp cannot be null or empty for conversion.");
        try
        {
            return double.Parse(timeStamp).ConvertToDateTimeFromUnixTimestamp();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error converting '{timeStamp}' to DateTime.", ex);
        }
    }

    public static DateTime ConvertToDateTimeFromUnixTimestamp(this double timeStamp)
    {
        try
        {
            return new DateTime(1970, 1, 1).AddSeconds(timeStamp).ToLocalTime();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error converting '{timeStamp}' to DateTime.", ex);
        }
    }

    public static DateTime? ConvertToDateTimeFromUnixTimestampIfCanNotConvertThenReturnDefaultValue(this string? timeStamp, DateTime? defaultValue = null)
    {
        try
        {
            return timeStamp.ConvertToDateTimeFromUnixTimestamp();
        }
        catch
        {
            return defaultValue;
        }
    }

    #endregion

    #region DateTime Manipulation

    public static DateTime WithTime(this DateTime dt, string time, string separator = ":")
    {
        try
        {
            var parts = time.Split(separator);
            int hour = int.Parse(parts[0]);
            int minute = int.Parse(parts[1]);
            return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, 0);
        }
        catch
        {
            throw new NotValidDataException($"Cannot convert '{time}' to a valid time.");
        }
    }

    #endregion

    #region DateTime Comparison

    public static bool IsThisDay(this DateTime dt, DateTime otherDay) => dt.Date == otherDay.Date;

    #endregion
}