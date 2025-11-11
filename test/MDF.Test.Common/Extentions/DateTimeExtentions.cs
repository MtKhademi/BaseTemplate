namespace MDF.Test.Common.Extentions;

public static class DateTimeExtentions
{
    public static string GetISOStringDateTime(this DateTime dt)
    {
        return $"{dt.Year,0000}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}T{dt.Hour.ToString("00")}:{dt.Minute.ToString("00")}:{dt.Second.ToString("00")}";
    }
    public static string GetISOStringDate(this DateTime dt)
    {
        return $"{dt.Year,0000}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";
    }
    public static string GetISOStringDate2(this DateTime dt)
    {
        return $"{dt.Year,0000}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}T00:00:00";
    }

    public static DateTime GetDateTimeFromISOFormat(this string dt)
    {
        return dt.ConvertToDateTimeFromMiladiDateTime("T", "-", ":");
    }
    public static DateTime? TryGetDateTimeFromISOFormat(this string dt)
    {
        if (string.IsNullOrWhiteSpace(dt)) return null;
        return dt.ConvertToDateTimeFromMiladiDateTime("T", "-", ":");
    }


    public static DateTime GetDateFromISOFormat(this string dt)
    {
        return dt.ConvertToDateFromMiladiDate("-", "T", ":");
    }
    public static DateTime? TryGetDateFromISOFormat(this string? dt)
    {
        if (string.IsNullOrWhiteSpace(dt)) return null;
        return dt.ConvertToDateFromMiladiDate("T", "-", ":");
    }
}
