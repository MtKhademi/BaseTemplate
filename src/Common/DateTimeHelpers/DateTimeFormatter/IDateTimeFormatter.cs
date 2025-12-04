namespace Infrastructure.DateTimeHelpers.DateTimeFormatter;

public interface IDateTimeFormatter
{
    // Validation methods
    bool IsValidDateTime(string input);
    bool IsValidDate(string input);
    bool IsValidTime(string input);

    // Formatting methods
    string? FormatDateTime(DateTime? dateTime);
    string FormatDateTime(DateTime dateTime);
    string? FormatDate(DateTime? dateTime);
    string FormatDate(DateTime dateTime);
    string? FormatTime(DateTime? dateTime);
    string FormatTime(DateTime dateTime);

    // Parsing methods
    DateTime? ParseDateTime(string? input);
    DateTime ParseDateTimeOrThrow(string input);
    DateTime? ParseDate(string? input);
    DateTime ParseDateOrThrow(string input);

}