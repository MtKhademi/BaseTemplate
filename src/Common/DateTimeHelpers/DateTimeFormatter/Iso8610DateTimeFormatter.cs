namespace Common.DateTimeHelpers.DateTimeFormatter;

public class Iso8610DateTimeFormatter : IDateTimeFormatter
{
    public const string BetweenDateAndTime = "T";
    public const string DateSeparator = "-";
    public const string TimeSeparator = ":";

    // Validation methods
    public bool IsValidDateTime(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;
        try
        {
            ParseDateTimeOrThrow(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsValidDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;
        try
        {
            ParseDateOrThrow(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsValidTime(string input)
    {
        // Not implemented
        throw new NotImplementedException();
    }

    // Formatting methods
    public string? FormatDateTime(DateTime? dateTime) =>
        dateTime.HasValue ? FormatDateTime(dateTime.Value) : null;

    public string FormatDateTime(DateTime dateTime) =>
        dateTime.GetStringDateTime(
            dateSeparator: DateSeparator,
            dateTimeSeparator: BetweenDateAndTime,
            timeSeparator: TimeSeparator);

    public string? FormatDate(DateTime? dateTime) =>
        dateTime.HasValue ? FormatDate(dateTime.Value) : null;

    public string FormatDate(DateTime dateTime) =>
        dateTime.GetStringDate(dateSeparator: DateSeparator);

    public string? FormatTime(DateTime? dateTime) =>
        dateTime.HasValue ? FormatTime(dateTime.Value) : null;

    public string FormatTime(DateTime dateTime) =>
        dateTime.GetStringTime(timeSeparator: TimeSeparator);

    // Parsing methods
    public DateTime? ParseDateTime(string? input) =>
        string.IsNullOrWhiteSpace(input) ? null : ParseDateTimeOrThrow(input);

    public DateTime ParseDateTimeOrThrow(string input) =>
        input.ConvertToDateTimeFromMiladiDateTime(
            dateTimeSeparator: BetweenDateAndTime,
            dateSeparator: DateSeparator,
            timeSeparator: TimeSeparator);

    public DateTime? ParseDate(string? input) =>
        string.IsNullOrWhiteSpace(input) ? null : ParseDateOrThrow(input);

    public DateTime ParseDateOrThrow(string input) =>
        input.ConvertToDateFromMiladiDate(dateSeparator: DateSeparator);
}